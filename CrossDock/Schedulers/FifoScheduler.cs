using CrossDock.Models;
using System;
using CrossDock.Parameters;
using System.Collections;
using System.Linq;

namespace CrossDock.Schedulers
{
    public class FifoScheduler : IScheduler
    {
        public Bee Reschedule(TransportationPlan plan, Bee bee, int time)
        {
            throw new NotImplementedException();
        }

        public Bee Schedule(TransportationPlan plan)
        {
            // Columns number : 4 (0:dock ID, 1:worker team ID, 2:start time, 3:end time)
            int[,] scheduleUnloading = new int[ParametersValues.Instance.NumberOfInboundTrucks, 4];
            int[,] scheduleLoading = new int[ParametersValues.Instance.NumberOfOutboundTrucks, 4];

            // Sort unloading tasks array in the order from the earliest coming to the last one
            Array.Sort(plan.UnloadingTasks, new CompareTaskTime());

            // For holding free time for each resource
            int[] inboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfInboundDocks];
            int[] outboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfOutboundDocks];
            int[] workersFreeTime = new int[ParametersValues.Instance.NumberOfWorkers];

            Random random = new Random();
            int[] isUnloadingTaskScheduled = new int[ParametersValues.Instance.NumberOfInboundTrucks];
            int[] isLoadingTaskScheduled = new int[ParametersValues.Instance.NumberOfOutboundTrucks];

            // Loop over all inbound tasks to schedule them
            for (int queueIterator = 0; queueIterator < ParametersValues.Instance.NumberOfInboundTrucks; queueIterator++)
            {

                int unloadingTaskID = plan.UnloadingTasks[queueIterator].Id;
                // Schedule one unloading task and get the row with all information for the schedule
                int[] row = ScheduleOneUnloading(plan, random, unloadingTaskID, inboundDocksFreeTime, workersFreeTime);

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
                    int arrivalTimeOut = CheckIfDemandMet(plan, scheduleUnloading, loadingTaskID, unloadingTaskID, 
                                                            isLoadingTaskScheduled, isUnloadingTaskScheduled);
                    if (arrivalTimeOut != 0)
                    {
                        int[] rowOut = ScheduleOneLoading(plan, random, loadingTaskID, outboundDocksFreeTime, workersFreeTime, arrivalTimeOut);

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

            return new Bee(scheduleUnloading, scheduleLoading);
        }

        public int[] ScheduleOneUnloading(TransportationPlan plan, Random random, int taskId, int[] inboundDocksFreeTime, int[] workersFreeTime)
        {
            int arrivalTime = plan.UnloadingTasks[taskId].ArrivalTime;
            int proceedingTime = plan.UnloadingTasks[taskId].ProductsAmount * ParametersValues.Instance.TimePerProductUnit;

            // Draw random resources (Dock and Worker team) which will be used for this task 
            int randomInDockID = random.Next(ParametersValues.Instance.NumberOfInboundDocks);
            int randomWorkerID = random.Next(ParametersValues.Instance.NumberOfWorkers);

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

        public int[] ScheduleOneLoading(TransportationPlan plan, Random random, int taskId, int[] outboundDocksFreeTime, int[] workersFreeTime, int arrivalTimeOut)
        {
            int proceedingTimeOut = plan.LoadingTasks[taskId].ProductsAmount * ParametersValues.Instance.TimePerProductUnit;

            // Draw random resources (Dock and Worker team) which will be used for this task 
            int randomOutDockID = random.Next(ParametersValues.Instance.NumberOfOutboundDocks);
            int randomWorkerIDout = random.Next(ParametersValues.Instance.NumberOfWorkers);

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

        public int CheckIfDemandMet(TransportationPlan plan, int[,] scheduleUnloading, int loadingTaskID, int unloadingTaskID, int[] isLoadingTaskScheduled, int[] isUnloadingTaskScheduled)
        {
            if (isLoadingTaskScheduled[loadingTaskID] == 0 && plan.LoadingTasks[loadingTaskID].Demand[unloadingTaskID] > 0)
            {
                int[] arrivalTimesOfDemand = new int[ParametersValues.Instance.NumberOfInboundTrucks];
                
                // Check each demanded product if alerady arrived
                for (int k = 0; k < ParametersValues.Instance.NumberOfInboundTrucks; k++)
                {
                    if (plan.LoadingTasks[loadingTaskID].Demand[k] > 0)
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

    public class CompareTaskTime : IComparer
    {
        // Compare for which task the inbound truck comes first.
        int IComparer.Compare(Object x, Object y)
        {
            UnloadingTask a = (UnloadingTask)x;
            UnloadingTask b = (UnloadingTask)y;
            if (a.ArrivalTime < b.ArrivalTime)
                return -1;
            else if (a.ArrivalTime > b.ArrivalTime)
                return 1;
            else
                return 0;
        }
    }
}
