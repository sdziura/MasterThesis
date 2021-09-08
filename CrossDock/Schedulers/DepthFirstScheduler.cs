using System;
using CrossDock.Models;
using CrossDock.Parameters;
using System.Linq;

namespace CrossDock.Schedulers
{
    public class DepthFirstScheduler
    {
       TransportationPlan plan;

        public DepthFirstScheduler(TransportationPlan plan)
        {
            this.plan = plan;
        }

        public Bee Schedule()
        {
            Bee bee = new Bee(plan, new int[ParametersValues.Instance.NumberOfInboundTrucks, 4], new int[ParametersValues.Instance.NumberOfOutboundTrucks, 4]);
            return FindBee(bee.Clone(), 
                new bool[ParametersValues.Instance.NumberOfInboundTrucks + ParametersValues.Instance.NumberOfOutboundTrucks], 
                bee.Clone(), 
                new int[ParametersValues.Instance.NumberOfWorkers], 
                new int[ParametersValues.Instance.NumberOfInboundDocks], 
                new int[ParametersValues.Instance.NumberOfOutboundDocks]).Clone();
        }
        private Bee FindBee(Bee currentBee, bool[] isSchedule, Bee bestBee, int[] workersTime, int[] inDocksTime, int[] outDocksTime)
        {
            // Choose task to schedule
            for (int taskIterator = 0; taskIterator < (ParametersValues.Instance.NumberOfInboundTrucks + ParametersValues.Instance.NumberOfOutboundTrucks); taskIterator++)
            {
                // Skip if already scheduled
                if (isSchedule[taskIterator])
                {
                    continue;
                }
                int productsReadyTime = 0;
                // Schedule loading task
                if (taskIterator >= ParametersValues.Instance.NumberOfInboundTrucks)
                {
                    bool isReady = true;
                    int[] arrivalTimesOfDemand = new int[ParametersValues.Instance.NumberOfInboundTrucks];
                    // Check each demanded product if alerady arrived
                    for (int k = 0; k < ParametersValues.Instance.NumberOfInboundTrucks; k++)
                    {
                        if (plan.LoadingTasks[taskIterator - ParametersValues.Instance.NumberOfInboundTrucks].Demand[k] > 0 && !isSchedule[k])
                        {
                            isReady = false;
                            break;
                        }
                        arrivalTimesOfDemand[k] = currentBee.ScheduleUnloading[k, 3];
                    }
                    if (!isReady)
                    {
                        continue;
                    }
                    else productsReadyTime = arrivalTimesOfDemand.Max();
                }
                for (int workerIterator = 0; workerIterator < ParametersValues.Instance.NumberOfWorkers; workerIterator++) 
                {
                    if (taskIterator >= ParametersValues.Instance.NumberOfInboundTrucks)
                    {
                        int outId = taskIterator - ParametersValues.Instance.NumberOfInboundTrucks;
                        for (int outDockIterator = 0; outDockIterator < ParametersValues.Instance.NumberOfOutboundDocks; outDockIterator++)
                        {
                            // Schedule out
                            int[] times = { productsReadyTime, workersTime[workerIterator], outDocksTime[outDockIterator] };
                            int timeStart = times.Max();
                            Bee newBee = currentBee.Clone();
                            bool[] newIsSchedule = (bool[])isSchedule.Clone();
                            int[] newWorkersTime = (int[])workersTime.Clone();
                            int[] newOutDocksTime = (int[])outDocksTime.Clone();

                            newBee.ScheduleLoading[outId, 0] = outDockIterator;
                            newBee.ScheduleLoading[outId, 1] = workerIterator;
                            newBee.ScheduleLoading[outId, 2] = timeStart;
                            newBee.ScheduleLoading[outId, 3] = timeStart + (plan.LoadingTasks[outId].ProductsAmount * ParametersValues.Instance.TimePerProductUnit);
                            
                            newOutDocksTime[outDockIterator] = newBee.ScheduleLoading[outId, 3];
                            newWorkersTime[workerIterator] = newBee.ScheduleLoading[outId, 3];
                            newIsSchedule[taskIterator] = true;
                            if (bestBee.TimeOfWork != 0 && newBee.ScheduleLoading[outId, 3] > bestBee.TimeOfWork)
                            {
                                continue;
                            }
                            if(newIsSchedule.All( x => x ))
                            {
                               if(newBee.CheckStorage()) bestBee = newBee.Clone();
                            }
                            else
                            {
                                bestBee = FindBee(newBee, newIsSchedule, bestBee, newWorkersTime, inDocksTime, newOutDocksTime).Clone();
                            }                        
                        }
                    }
                    else
                    {
                        for (int inDockIterator = 0; inDockIterator < ParametersValues.Instance.NumberOfInboundDocks; inDockIterator++)
                        {
                            // Schedule in
                            int[] times = { plan.UnloadingTasks[taskIterator].ArrivalTime, workersTime[workerIterator], inDocksTime[inDockIterator] };
                            int timeStart = times.Max();
                            Bee newBee = currentBee.Clone();
                            bool[] newIsSchedule = (bool[])isSchedule.Clone();
                            int[] newWorkersTime = (int[])workersTime.Clone();
                            int[] newInDocksTime = (int[])inDocksTime.Clone();

                            newBee.ScheduleUnloading[taskIterator, 0] = inDockIterator;
                            newBee.ScheduleUnloading[taskIterator, 1] = workerIterator;
                            newBee.ScheduleUnloading[taskIterator, 2] = timeStart;
                            newBee.ScheduleUnloading[taskIterator, 3] = timeStart + (plan.UnloadingTasks[taskIterator].ProductsAmount * ParametersValues.Instance.TimePerProductUnit);

                            newInDocksTime[inDockIterator] = newBee.ScheduleUnloading[taskIterator, 3];
                            newWorkersTime[workerIterator] = newBee.ScheduleUnloading[taskIterator, 3];
                            newIsSchedule[taskIterator] = true;

                            if (bestBee.TimeOfWork != 0 && newBee.ScheduleUnloading[taskIterator, 3] > bestBee.TimeOfWork) continue;
                            if (newIsSchedule.All(x => x))
                            {
                                if (newBee.CheckStorage()) bestBee = newBee.Clone();
                            }
                            else
                            {
                                bestBee = FindBee(newBee, newIsSchedule, bestBee, newWorkersTime, newInDocksTime, outDocksTime).Clone();
                            }
                        }
                    }
                }
            }
            return bestBee;
        }
    }
}
