using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Models
{
    public class Bee
    {
        private int[][][] _scheduleUnloading;
        private int[][][] _scheduleLoading;

        public Bee(int[][][] scheduleUnloading, int[][][] scheduleLoading)
        {
            _scheduleUnloading = scheduleUnloading;
            _scheduleLoading = scheduleLoading;
        }

        public int[][][] ScheduleUnloading { get => _scheduleUnloading; set => _scheduleUnloading = value; }
        public int[][][] ScheduleLoading { get => _scheduleLoading; set => _scheduleLoading = value; }
    }
}
