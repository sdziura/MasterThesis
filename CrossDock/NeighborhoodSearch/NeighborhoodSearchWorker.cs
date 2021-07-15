using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossDock.Models;
using CrossDock.Parameters;


namespace CrossDock.NeighborhoodSearch
{
    public class NeighborhoodSearchWorker : INeighborhoodSearch
    {
        public Bee SearchRegion(Bee bee, TransportationPlan transportationPlan)
        {
            Random random = new Random();

            // Get random index of worker that will be scheduled again
            int workerToChange = random.Next(ParametersValues.Instance.NumberOfWorkers);
            
            // Make arrays informing which tasks are scheduled. Initial value is 1 for each task.
            int[] isUnloadingScheduled = new int[ParametersValues.Instance.NumberOfInboundTrucks];
            int[] isLoadingScheduled = new int[ParametersValues.Instance.NumberOfOutboundTrucks];
            Array.Fill(isUnloadingScheduled, 1);
            Array.Fill(isLoadingScheduled, 1);

            // Deleting from schedule unloading tasks assigned to drawn worker
            for(int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                if (bee.ScheduleUnloading[i, 1] == workerToChange)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bee.ScheduleUnloading[i, j] = 0; 
                    }
                    isUnloadingScheduled[i] = 0;
                }
            }
            // Deleting from schedule loading tasks assigned to drawn worker
            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
            {
                if (bee.ScheduleLoading[i, 1] == workerToChange)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bee.ScheduleLoading[i, j] = 0;
                    }
                    isLoadingScheduled[i] = 0;
                }
            }

            // Find new Free times for docks
            int[] inboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfInboundDocks];
            int[] outboundDocksFreeTime = new int[ParametersValues.Instance.NumberOfOutboundDocks];
            for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                if(bee.ScheduleUnloading[i, 3] > inboundDocksFreeTime[bee.ScheduleUnloading[i, 0]])
                    inboundDocksFreeTime[bee.ScheduleUnloading[i, 0]] = bee.ScheduleUnloading[i, 3];
            }
            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
            {
                if (bee.ScheduleLoading[i, 3] > outboundDocksFreeTime[bee.ScheduleLoading[i, 0]])
                    outboundDocksFreeTime[bee.ScheduleLoading[i, 0]] = bee.ScheduleLoading[i, 3];
            }

            // Sort tasks
            UnloadingTask[] sortedUnloadingTasks = transportationPlan.UnloadingTasks.OrderBy(x => random.Next()).ToArray();
            LoadingTask[] sortedLoadingTasks = transportationPlan.LoadingTasks.OrderBy(x => random.Next()).ToArray();

            // Schedule unscheduled tasks
            int workerFreeTime = 0;
            for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                if (isUnloadingScheduled[sortedUnloadingTasks[i].Id] == 0)
                {
                    int inDockID = random.Next(ParametersValues.Instance.NumberOfInboundDocks);
                    int[] tempArray = { transportationPlan.UnloadingTasks[sortedUnloadingTasks[i].Id].ArrivalTime, inboundDocksFreeTime[inDockID], workerFreeTime };
                    int freeTime = tempArray.Max();

                    isUnloadingScheduled[sortedUnloadingTasks[i].Id] = 1;
                    bee.ScheduleUnloading[sortedUnloadingTasks[i].Id, 0] = inDockID;
                    bee.ScheduleUnloading[sortedUnloadingTasks[i].Id, 1] = workerToChange;
                    bee.ScheduleUnloading[sortedUnloadingTasks[i].Id, 2] = freeTime;
                    bee.ScheduleUnloading[sortedUnloadingTasks[i].Id, 3] = freeTime + transportationPlan.UnloadingTasks[sortedUnloadingTasks[i].Id].ProductsAmount * ParametersValues.Instance.TimePerProductUnit;

                    workerFreeTime = bee.ScheduleUnloading[sortedUnloadingTasks[i].Id, 3];
                    inboundDocksFreeTime[inDockID] = bee.ScheduleUnloading[sortedUnloadingTasks[i].Id, 3];

                }

                    // Schedule outbound tasks if ready
                for (int j = 0; j < ParametersValues.Instance.NumberOfOutboundTrucks; j++)
                {
                    int loadingTaskId = sortedLoadingTasks[j].Id;

                    if (isLoadingScheduled[loadingTaskId] == 0)
                    {   

                        int isDemandMet = 1;
                        int[] demendedProductsReadyTime = new int[ParametersValues.Instance.NumberOfInboundTrucks];
                        for(int k = 0; k < ParametersValues.Instance.NumberOfInboundTrucks; k++ )
                        {
                            if (isUnloadingScheduled[k] == 0 && 0 < transportationPlan.LoadingTasks[loadingTaskId].Demand[k])
                                isDemandMet = 0;
                            
                            else
                                demendedProductsReadyTime[k] = bee.ScheduleUnloading[k, 3];
                        }

                        if (isDemandMet == 0) break;
                        else
                        {
                            int outDockID = random.Next(ParametersValues.Instance.NumberOfOutboundDocks);
                            int timeOfDemandMet = demendedProductsReadyTime.Max();
                            int[] tempArrayOut = { timeOfDemandMet, outboundDocksFreeTime[outDockID], workerFreeTime };
                            int timeForTaskStart = tempArrayOut.Max();

                            bee.ScheduleLoading[loadingTaskId, 0] = outDockID;
                            bee.ScheduleLoading[loadingTaskId, 1] = workerToChange;
                            bee.ScheduleLoading[loadingTaskId, 2] = timeForTaskStart;
                            bee.ScheduleLoading[loadingTaskId, 3] = timeForTaskStart + transportationPlan.LoadingTasks[loadingTaskId].ProductsAmount * ParametersValues.Instance.TimePerProductUnit;
                            workerFreeTime = bee.ScheduleLoading[loadingTaskId, 3];
                            outboundDocksFreeTime[outDockID] = bee.ScheduleLoading[loadingTaskId, 3];
                            
                            isLoadingScheduled[loadingTaskId] = 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                
            }

            return bee;
        }
    }
}
