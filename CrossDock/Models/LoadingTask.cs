using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Models
{
    public class LoadingTask
    {
        private int _id;
        private int[] _demand;

        public LoadingTask(int id, int[] demand)
        {
            _id = id;
            _demand = demand;
        }

        public int Id { get => _id; }
        public int[] Demand { get => _demand; set => _demand = value; }
    }
}
