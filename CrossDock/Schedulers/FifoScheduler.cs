using CrossDock.Models;
using System;
using CrossDock.Parameters;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace CrossDock.Schedulers
{
    public class FifoScheduler : IScheduler
    {
        private TransportationPlan _plan;
        private Random _random;
        private IComparer<UnloadingTask> _unloadingComparer;
        private IComparer<LoadingTask> _loadingComparer;
        

        public TransportationPlan Plan { get => _plan; set => _plan = value; }
        public IComparer<UnloadingTask> UnloadingComparer { get => _unloadingComparer; set => _unloadingComparer = value; }
        public IComparer<LoadingTask> LoadingComparer { get => _loadingComparer; set => _loadingComparer = value; }

        public FifoScheduler() 
        { }
        public FifoScheduler(TransportationPlan plan)
        {
            this._plan = plan;
            this._random = new Random();
            _unloadingComparer = new CompareTaskRandom<UnloadingTask>();
            _loadingComparer = new CompareTaskRandom<LoadingTask>();
        }
        public FifoScheduler(TransportationPlan plan, IComparer<UnloadingTask> unloadingComparer, IComparer<LoadingTask> loadingComaprer)
        {
            this._plan = plan;
            this._random = new Random();
            _unloadingComparer = unloadingComparer;
            _loadingComparer = loadingComaprer;
        }


        public Bee Reschedule( Bee bee, int time)
        {
            
            // Get tasks from Transportation Plan and sort them
            UnloadingTask[] sortedUnloadingTasks = new UnloadingTask[ParametersValues.Instance.NumberOfInboundTrucks]; 
            LoadingTask[] sortedLoadingTasks = new LoadingTask[ParametersValues.Instance.NumberOfOutboundTrucks]; 
            Array.Copy(Plan.UnloadingTasks, sortedUnloadingTasks, ParametersValues.Instance.NumberOfInboundTrucks);
            Array.Copy(Plan.LoadingTasks, sortedLoadingTasks, ParametersValues.Instance.NumberOfOutboundTrucks);
            Array.Sort(sortedUnloadingTasks, UnloadingComparer);
            Array.Sort(sortedLoadingTasks, LoadingComparer);

            // Make arrays informing which tasks are scheduled. Initial value is 1 for each task.
            int[] isUnloadingScheduled = new int[ParametersValues.Instance.NumberOfInboundTrucks];
            int[] isLoadingScheduled = new int[ParametersValues.Instance.NumberOfOutboundTrucks];
            Array.Fill(isUnloadingScheduled, 1);
            Array.Fill(isLoadingScheduled, 1);
            
            // Removing from schedule tasks that are not yet started
            for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                if(bee.ScheduleUnloading[i, 2] > time)
                {
                    for (int j = 0; j < 4; j++) bee.ScheduleUnloading[i, j] = 0;
                    isUnloadingScheduled[i] = 0;
                }
            }
            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
            {
                if (bee.ScheduleLoading[i, 2] > time)
                {
                    for (int j = 0; j < 4; j++) bee.ScheduleLoading[i, j] = 0;
                    isLoadingScheduled[i] = 0;
                }
            }

            // Find new FreeTimes for resources
            int[] inboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfInboundDocks];
            Array.Fill(inboundDocksFreeTime, time);
            int[] outboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfOutboundDocks];
            Array.Fill(outboundDocksFreeTime, time);
            int[] workersFreeTime = new int[ParametersValues.Instance.NumberOfWorkers];
            Array.Fill(workersFreeTime, time);

            for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                if (bee.ScheduleUnloading[i, 3] > inboundDocksFreeTime[bee.ScheduleUnloading[i, 0]])
                    inboundDocksFreeTime[bee.ScheduleUnloading[i, 0]] = bee.ScheduleUnloading[i, 3];
                if (bee.ScheduleUnloading[i, 3] > workersFreeTime[bee.ScheduleUnloading[i, 1]])
                    workersFreeTime[bee.ScheduleUnloading[i, 1]] = bee.ScheduleUnloading[i, 3];
            }
            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
            {
                if (bee.ScheduleLoading[i, 3] > outboundDocksFreeTime[bee.ScheduleLoading[i, 0]])
                    outboundDocksFreeTime[bee.ScheduleLoading[i, 0]] = bee.ScheduleLoading[i, 3];
                if (bee.ScheduleLoading[i, 3] > workersFreeTime[bee.ScheduleLoading[i, 1]])
                    workersFreeTime[bee.ScheduleLoading[i, 1]] = bee.ScheduleLoading[i, 3];
            }

            // Schedule back
            for (int queueIterator = 0; queueIterator < ParametersValues.Instance.NumberOfInboundTrucks; queueIterator++)
            {
                int unloadingTaskID = sortedUnloadingTasks[queueIterator].Id;

                if (isUnloadingScheduled[unloadingTaskID] != 1)
                {
                    // Schedule one unloading task and get the row with all information for the schedule
                    int[] row = ScheduleOneUnloading(unloadingTaskID, inboundDocksFreeTime, workersFreeTime);

                    // Sign task and resources to schedule
                    bee.ScheduleUnloading[unloadingTaskID, 0] = row[0];
                    bee.ScheduleUnloading[unloadingTaskID, 1] = row[1];
                    bee.ScheduleUnloading[unloadingTaskID, 2] = row[2];
                    bee.ScheduleUnloading[unloadingTaskID, 3] = row[3];

                    // Update free time of dock and worker team
                    inboundDocksFreeTime[row[0]] = row[3];
                    workersFreeTime[row[1]] = row[3];

                    isUnloadingScheduled[unloadingTaskID] = 1;
                }

                // Check if the outbound task can be scheduled !isLoadingScheduled.All(x => x==1)
                for (int loadingTaskIterator = 0; loadingTaskIterator < ParametersValues.Instance.NumberOfOutboundTrucks; loadingTaskIterator++)
                {
                    int loadingTaskID = sortedLoadingTasks[loadingTaskIterator].Id;
                    if (isLoadingScheduled[loadingTaskID] == 1) continue;
                    // returns time when demend is met, or 0 when its still unknown
                    int arrivalTimeOut = CheckIfDemandMet(bee.ScheduleUnloading, loadingTaskID,
                                                            isLoadingScheduled, isUnloadingScheduled);
                    if (arrivalTimeOut != 0)
                    {
                        int[] rowOut = ScheduleOneLoading(loadingTaskID, outboundDocksFreeTime, workersFreeTime, arrivalTimeOut);

                        // Sign task and resources to schedule
                        bee.ScheduleLoading[loadingTaskID, 0] = rowOut[0];
                        bee.ScheduleLoading[loadingTaskID, 1] = rowOut[1];
                        bee.ScheduleLoading[loadingTaskID, 2] = rowOut[2];
                        bee.ScheduleLoading[loadingTaskID, 3] = rowOut[3];

                        // Update free time of dock and worker team
                        outboundDocksFreeTime[rowOut[0]] = rowOut[3];
                        workersFreeTime[rowOut[1]] = rowOut[3];

                        isLoadingScheduled[loadingTaskID] = 1;
                        //continue;
                    }
                    else break;
                }
            }
            return bee;
        }

        public Bee Schedule()
        {
            UnloadingTask[] sortedUnloadingTasks = new UnloadingTask[ParametersValues.Instance.NumberOfInboundTrucks];
            LoadingTask[] sortedLoadingTasks = new LoadingTask[ParametersValues.Instance.NumberOfOutboundTrucks];
            Array.Copy(Plan.UnloadingTasks, sortedUnloadingTasks, ParametersValues.Instance.NumberOfInboundTrucks);
            Array.Copy(Plan.LoadingTasks, sortedLoadingTasks, ParametersValues.Instance.NumberOfOutboundTrucks);
            Array.Sort(sortedUnloadingTasks, UnloadingComparer);
            //Array.Sort(sortedLoadingTasks, LoadingComparer);
            sortedLoadingTasks.OrderBy(x => _random.Next()).ToArray();

            // Columns number : 4 (0:dock ID, 1:worker team ID, 2:start time, 3:end time)
            int[,] scheduleUnloading = new int[ParametersValues.Instance.NumberOfInboundTrucks, 4];
            int[,] scheduleLoading = new int[ParametersValues.Instance.NumberOfOutboundTrucks, 4];

            // For holding free time for each resource
            int[] inboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfInboundDocks];
            int[] outboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfOutboundDocks];
            int[] workersFreeTime = new int[ParametersValues.Instance.NumberOfWorkers];

            
            int[] isUnloadingTaskScheduled = new int[ParametersValues.Instance.NumberOfInboundTrucks];
            int[] isLoadingTaskScheduled = new int[ParametersValues.Instance.NumberOfOutboundTrucks];

            // Loop over all inbound tasks to schedule them
            for (int queueIterator = 0; queueIterator < ParametersValues.Instance.NumberOfInboundTrucks; queueIterator++)
            {

                int unloadingTaskID = sortedUnloadingTasks[queueIterator].Id;
                // Schedule one unloading task and get the row with all information for the schedule
                int[] row = ScheduleOneUnloading( unloadingTaskID, inboundDocksFreeTime, workersFreeTime);

                // Sign task and resources to schedule
                scheduleUnloading[unloadingTaskID, 0] = row[0];
                scheduleUnloading[unloadingTaskID, 1] = row[1];
                scheduleUnloading[unloadingTaskID, 2] = row[2];
                scheduleUnloading[unloadingTaskID, 3] = row[3];

                // Update free time of dock and worker team
                inboundDocksFreeTime[row[0]] = row[3];
                workersFreeTime[row[1]] = row[3];

                isUnloadingTaskScheduled[unloadingTaskID] = 1;

                // Check if the outbound task can be scheduled
                for (int loadingTaskID = 0; loadingTaskID < ParametersValues.Instance.NumberOfOutboundTrucks; loadingTaskID++)
                {
                    // returns time when demend is met, or 0 when its still unknown
                    int arrivalTimeOut = CheckIfDemandMet( scheduleUnloading, loadingTaskID, 
                                                            isLoadingTaskScheduled, isUnloadingTaskScheduled);
                    if (arrivalTimeOut != 0)
                    {
                        int[] rowOut = ScheduleOneLoading( loadingTaskID, outboundDocksFreeTime, workersFreeTime, arrivalTimeOut);

                        // Sign task and resources to schedule
                        scheduleLoading[loadingTaskID, 0] = rowOut[0];
                        scheduleLoading[loadingTaskID, 1] = rowOut[1];
                        scheduleLoading[loadingTaskID, 2] = rowOut[2];
                        scheduleLoading[loadingTaskID, 3] = rowOut[3];

                        // Update free time of dock and worker team
                        outboundDocksFreeTime[rowOut[0]] = rowOut[3];
                        workersFreeTime[rowOut[1]] = rowOut[3];

                        isLoadingTaskScheduled[loadingTaskID] = 1;
                    }            
                }
            }
            return new Bee(Plan, scheduleUnloading, scheduleLoading);
        }

        public int[] ScheduleOneUnloading( int taskId, int[] inboundDocksFreeTime, int[] workersFreeTime)
        {
            int arrivalTime = Plan.UnloadingTasks[taskId].ArrivalTime;
            int proceedingTime = Plan.UnloadingTasks[taskId].ProductsAmount * ParametersValues.Instance.TimePerProductUnit;

            // Draw random resources (Dock and Worker team) which will be used for this task 
            int randomInDockID = _random.Next(ParametersValues.Instance.NumberOfInboundDocks);
            int randomWorkerID = _random.Next(ParametersValues.Instance.NumberOfWorkers);

            // Get earliest common free time for chosen dock, worker team and time of arrival
            int[] tempArray = { arrivalTime, inboundDocksFreeTime[randomInDockID], workersFreeTime[randomWorkerID] };
            int freeTime = tempArray.Max();

            int[] resultRow = new int[4];
            resultRow[0] = randomInDockID;
            resultRow[1] = randomWorkerID;
            resultRow[2] = freeTime;
            resultRow[3] = freeTime + proceedingTime;

            return resultRow;
        }

        private int[] ScheduleOneLoading(int taskId, int[] outboundDocksFreeTime, int[] workersFreeTime, int arrivalTimeOut)
        {
            int proceedingTimeOut = Plan.LoadingTasks[taskId].ProductsAmount * ParametersValues.Instance.TimePerProductUnit;

            // Draw random resources (Dock and Worker team) which will be used for this task 
            int randomOutDockID = _random.Next(ParametersValues.Instance.NumberOfOutboundDocks);
            int randomWorkerIDout = _random.Next(ParametersValues.Instance.NumberOfWorkers);

            // Get earliest common free time for chosen dock, worker team and time of arrival
            int[] tempArrayOut = { arrivalTimeOut, outboundDocksFreeTime[randomOutDockID], workersFreeTime[randomWorkerIDout] };
            int freeTimeOut = tempArrayOut.Max();

            // Sign task and resources to schedule
            int[] resultRow = new int[4];
            resultRow[0] = randomOutDockID;
            resultRow[1] = randomWorkerIDout;
            resultRow[2] = freeTimeOut;
            resultRow[3] = freeTimeOut + proceedingTimeOut;

            return resultRow;
        }

        // deleted from paremeters: "int unloadingTaskID," regarding change in code
        private int CheckIfDemandMet( int[,] scheduleUnloading, int loadingTaskIterator, int[] isLoadingTaskScheduled, int[] isUnloadingTaskScheduled)
        {
            if (isLoadingTaskScheduled[Plan.LoadingTasks[loadingTaskIterator].Id] == 0)// && plan.LoadingTasks[loadingTaskID].Demand[unloadingTaskID] > 0)
            {
                int[] arrivalTimesOfDemand = new int[ParametersValues.Instance.NumberOfInboundTrucks];
                
                // Check each demanded product if alerady arrived
                for (int k = 0; k < ParametersValues.Instance.NumberOfInboundTrucks; k++)
                {
                    if (Plan.LoadingTasks[loadingTaskIterator].Demand[k] > 0)
                    {
                        if (isUnloadingTaskScheduled[k] == 0) return 0;
                        arrivalTimesOfDemand[k] = scheduleUnloading[k, 3];
                    }
                }
                return arrivalTimesOfDemand.Max();
            }
            return 0;
        }
    }

    public class CompareTaskTime: IComparer<UnloadingTask>
    {
        // Compare for which task the inbound truck comes first.
        public int Compare(UnloadingTask x, UnloadingTask y)
        {
            if (x.ArrivalTime < y.ArrivalTime)
                return -1;
            else if (x.ArrivalTime > y.ArrivalTime)
                return 1;
            else
                return 0;
        }
    }

    public class CompareTaskRandom<T> : IComparer<T>
    {
        // Compare for which task the inbound truck comes first.
        public int Compare(T x, T y)
        {
            Random ran = new Random();
            return ran.Next(-1,1);
        }
    }
}
