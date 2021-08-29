using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossDock.Schedulers;
using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Models;
using CrossDock.Parameters;

namespace CrossDock.Schedulers.Tests
{
    [TestClass()]
    public class DepthFirstSchedulerTests
    {
        [TestMethod()]
        public void ScheduleTest()
        {
            ParametersValues.Instance.NumberOfInboundTrucks = 1;
            ParametersValues.Instance.NumberOfOutboundTrucks = 1;
            ParametersValues.Instance.NumberOfWorkers = 1;
            ParametersValues.Instance.NumberOfOutboundDocks = 1;
            ParametersValues.Instance.NumberOfInboundDocks = 1;



            UnloadingTask t0 = new UnloadingTask(0, 5, 6);
           // UnloadingTask t1 = new UnloadingTask(1, 10, 3);
           // UnloadingTask t2 = new UnloadingTask(2, 4, 7);

            LoadingTask l0 = new LoadingTask(0, new int[] { 6 });// 2, 0 });
            //LoadingTask l1 = new LoadingTask(1, new int[] { 0, 1, 0 });
            //LoadingTask l2 = new LoadingTask(2, new int[] { 1, 0, 7 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0 }, new LoadingTask[] { l0});
            DepthFirstScheduler sched = new DepthFirstScheduler(plan);
            Bee bee = sched.Schedule();
            bee.PrintSchedule();
        }
    }
}