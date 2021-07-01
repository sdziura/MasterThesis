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
    }
}
