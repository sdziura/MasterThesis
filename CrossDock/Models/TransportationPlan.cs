using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Parameters;

namespace CrossDock.Models
{
    public class TransportationPlan
    {
        private UnloadingTask[] _unloadingTasks;
        private LoadingTask[] _loadingTasks;

        public TransportationPlan() : 
            this(new int[ParametersValues.Instance.NumberOfInboundTrucks], new int[ParametersValues.Instance.NumberOfOutboundTrucks, ParametersValues.Instance.NumberOfInboundTrucks])
        {}
        public TransportationPlan( int[] arrivalTimes, int[,] outboundDemand)
        {
            if (arrivalTimes.Length != ParametersValues.Instance.NumberOfInboundTrucks)
                Console.WriteLine("Wrong number of arrival times");
            else if (outboundDemand.GetLength(0) != ParametersValues.Instance.NumberOfOutboundTrucks)
                Console.WriteLine("Wrong number of outbound trucks");
            else if (outboundDemand.GetLength(1) != ParametersValues.Instance.NumberOfInboundTrucks)
                Console.WriteLine("Wrong number of inbound trucks");
            else
            {
                _loadingTasks = new LoadingTask[ParametersValues.Instance.NumberOfOutboundTrucks];
                _unloadingTasks = new UnloadingTask[ParametersValues.Instance.NumberOfInboundTrucks];
                for(int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
                {
                    int[] demand = new int[ParametersValues.Instance.NumberOfInboundTrucks];
                    for (int j = 0; j < ParametersValues.Instance.NumberOfInboundTrucks; j++)
                        demand[j] = outboundDemand[i, j];
                    _loadingTasks[i] = new LoadingTask(i, demand);
                }
                for(int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
                {
                    int productAmount = 0;
                    for (int j = 0; j < ParametersValues.Instance.NumberOfOutboundTrucks; j++)
                        productAmount += outboundDemand[j, i];
                    _unloadingTasks[i] = new UnloadingTask(i, arrivalTimes[i], productAmount);
                }

            }
            
        }
        public TransportationPlan( UnloadingTask[] unloadingTasks, LoadingTask[] loadingTasks)
        {
            if (unloadingTasks.Length == ParametersValues.Instance.NumberOfInboundTrucks && loadingTasks.Length == ParametersValues.Instance.NumberOfOutboundTrucks)
            {
                _unloadingTasks = unloadingTasks;
                _loadingTasks = loadingTasks;
            }
            else Console.WriteLine("Wrong number of tasks in transportation plan");
        }

        public UnloadingTask[] UnloadingTasks { get => _unloadingTasks; set => _unloadingTasks = value; }
        public LoadingTask[] LoadingTasks { get => _loadingTasks; set => _loadingTasks = value; }
        public int[] ArrivalTimes 
        { 
            get 
            {
                int[] arrivalTimes = new int[ParametersValues.Instance.NumberOfInboundTrucks];
                for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
                    arrivalTimes[i] = UnloadingTasks[i].ArrivalTime;
                return arrivalTimes;
            } 
        }
        public int[,] Demand
        {
            get
            {
                int[,] demand = new int[ParametersValues.Instance.NumberOfOutboundTrucks, ParametersValues.Instance.NumberOfInboundTrucks];
                for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
                    for (int j = 0; j < ParametersValues.Instance.NumberOfInboundTrucks; j++)
                        demand[i, j] = LoadingTasks[i].Demand[j];
                return demand;
            }
        }

        public TransportationPlan Clone()
        {
            return new TransportationPlan(ArrivalTimes, Demand);
        }
    }
}
