using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Models
{
    class TransportationPlan
    {
        private int[] _unloadingTasks;
        private int[] _loadingTasks;

        public TransportationPlan(int[] unloadingTasks, int[] loadingTasks)
        {
            _unloadingTasks = unloadingTasks;
            _loadingTasks = loadingTasks;
        }

        public int[] UnloadingTasks { get => _unloadingTasks; set => _unloadingTasks = value; }
        public int[] LoadingTasks { get => _loadingTasks; set => _loadingTasks = value; }
    }
}
