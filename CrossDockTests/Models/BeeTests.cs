using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossDock.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Schedulers;
using System.Collections;
using CrossDock.Parameters;

namespace CrossDock.Models.Tests
{
    [TestClass()]
    public class BeeTests
    {
        public void Setup()
        {

            ParametersValues.Instance.MaxStorageCapacity = 1;
            ParametersValues.Instance.NumberOfWorkers = 2;
            ParametersValues.Instance.NumberOfInboundDocks = 2;
            ParametersValues.Instance.NumberOfOutboundDocks = 1;
            ParametersValues.Instance.NumberOfInboundTrucks = 5;
            ParametersValues.Instance.NumberOfOutboundTrucks = 3;
            ParametersValues.Instance.ScoutBeesNumber = 1;
            ParametersValues.Instance.SelectedRegionsNumber = 1;
            ParametersValues.Instance.EliteRegionsNumber = 1;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 1;
            ParametersValues.Instance.EliteRegionBeesNumber = 1;
            ParametersValues.Instance.TimePerProductUnit = 1;
        }

        [TestMethod()]
        public void CheckStorageTest()
        {
            Setup();

            UnloadingTask t0 = new UnloadingTask(0, 5, 6);
            UnloadingTask t1 = new UnloadingTask(1, 10, 3);
            UnloadingTask t2 = new UnloadingTask(2, 4, 7);
            UnloadingTask t3 = new UnloadingTask(3, 6, 2);
            UnloadingTask t4 = new UnloadingTask(4, 5, 9);

            LoadingTask l0 = new LoadingTask(0, new int[] { 5, 2, 0, 0, 3 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 0, 1, 0, 1, 3 });
            LoadingTask l2 = new LoadingTask(2, new int[] { 1, 0, 7, 1, 3 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1, t2, t3, t4 }, new LoadingTask[] { l0, l1, l2 });
            IComparer comparer = new CompareTaskTime();
            FifoScheduler sched = new FifoScheduler(plan);
            Bee bee = sched.Schedule(comparer);
            Console.WriteLine("\nNumber of iterations: " + (ParametersValues.Instance.NumberOfInboundTrucks + ParametersValues.Instance.NumberOfOutboundTrucks));
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(bee.ScheduleUnloading[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(bee.ScheduleLoading[i, j] + " ");
                Console.WriteLine();
            }

            Console.WriteLine("\n\n\nCheck Storage1:");
            Assert.IsFalse(bee.CheckStorage());

            Console.WriteLine("\n\n\nCheck Storage2:\n");
            ParametersValues.Instance.MaxStorageCapacity = 20;
            Assert.IsFalse(bee.CheckStorage());

            Console.WriteLine("\n\n\nCheck Storage3:\n");
            ParametersValues.Instance.MaxStorageCapacity = 27;
            Assert.IsTrue(bee.CheckStorage());
        }
    }
}