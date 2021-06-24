using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Models
{
    public class TransportationPlan
    {
        private UnloadingTask[] _unloadingTasks;
        private LoadingTask[] _loadingTasks;

        public TransportationPlan( UnloadingTask[] unloadingTasks, LoadingTask[] loadingTasks)
        {
            _unloadingTasks = unloadingTasks;
            _loadingTasks = loadingTasks;
        }

        public UnloadingTask[] UnloadingTasks { get => _unloadingTasks; set => _unloadingTasks = value; }
        public LoadingTask[] LoadingTasks { get => _loadingTasks; set => _loadingTasks = value; }
    }
}
