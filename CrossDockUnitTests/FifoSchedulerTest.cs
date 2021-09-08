using NUnit.Framework;
using CrossDock.Parameters;
using System.IO;
using System;
using CrossDock.Schedulers;
using CrossDock.Models;
using System.Collections;
using System.Collections.Generic;

namespace CrossDockUnitTests
{
    public class FifoSchedulerTests : FifoScheduler
    {
        [SetUp]
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

        [Test]
        public void ScheduleOneUnloadingTest()
        {
            UnloadingTask t0 = new UnloadingTask(0, 5, 6);
            UnloadingTask t1 = new UnloadingTask(1, 10, 3);
            UnloadingTask t2 = new UnloadingTask(2, 4, 7);
            UnloadingTask t3 = new UnloadingTask(3, 6, 2);
            UnloadingTask t4 = new UnloadingTask(4, 5, 9);

            LoadingTask l0 = new LoadingTask(0, new int[] { 1, 0, 0, 0, 0 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 0, 1, 0, 0, 0 });
            LoadingTask l2 = new LoadingTask(2, new int[] { 1, 0, 0, 0, 0 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1, t2, t3, t4 }, new LoadingTask[] { l0, l1, l2 });
            FifoScheduler sched = new FifoScheduler(plan);
            int[] result = sched.ScheduleOneUnloading( 2, new int[] { 2, 3 }, new int[] { 8, 9 });
            Assert.GreaterOrEqual(result[2], 8);
            Assert.LessOrEqual(result[2], 9);
            Console.WriteLine(result[0]);
            Console.WriteLine(result[1]);
            Console.WriteLine(result[2]);
            Console.WriteLine(result[3]);
        }

        [Test]
        public void IsDemandMetTest()
        {
            UnloadingTask t0 = new UnloadingTask(0, 5, 6);
            UnloadingTask t1 = new UnloadingTask(1, 10, 3);
            UnloadingTask t2 = new UnloadingTask(2, 4, 7);
            UnloadingTask t3 = new UnloadingTask(3, 6, 2);
            UnloadingTask t4 = new UnloadingTask(4, 5, 9);

            LoadingTask l0 = new LoadingTask(0, new int[] { 5, 2, 0, 0, 3 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 0, 1, 0, 1, 3 });
            LoadingTask l2 = new LoadingTask(2, new int[] { 1, 0, 7, 1, 3 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1, t2, t3, t4 }, new LoadingTask[] { l0, l1, l2 });
            FifoScheduler sched = new FifoScheduler(plan);
            int[,] scheduleUnloading = new int[5, 4];

            scheduleUnloading = new int[,]{    { 0, 0, 13, 19},
                                                { 0, 0, 10, 13},
                                                { 0, 0, 0, 0},
                                                { 1, 1, 6, 8},
                                                { 1, 1, 8, 17} };

            //int resfail = sched.CheckIfDemandMet(plan, scheduleUnloading, 0, 3, new int[] { 0, 0, 0 }, new int[] { 0, 1, 0, 1, 1 });
            //int respass = sched.CheckIfDemandMet(plan, scheduleUnloading, 1, 3, new int[] { 0, 0, 0 }, new int[] { 0, 1, 0, 1, 1 });
            //int resfail2 = sched.CheckIfDemandMet(plan, scheduleUnloading, 2, 3, new int[] { 0, 0, 0 }, new int[] { 0, 1, 0, 1, 1 });

            //int respass2 = sched.CheckIfDemandMet(scheduleUnloading, 0, 1, new int[] { 0, 0, 1 }, new int[] { 1, 1, 0, 1, 1 });

           // Assert.AreEqual(13, respass);
           // Assert.AreEqual(0, resfail);
           // Assert.AreEqual(0, resfail2);
           // Assert.AreEqual(19, respass2);
        }

        [Test]
        public void ScheduleOneLoadingTest()
        {
            UnloadingTask t0 = new UnloadingTask(0, 5, 6);
            UnloadingTask t1 = new UnloadingTask(1, 10, 3);
            UnloadingTask t2 = new UnloadingTask(2, 4, 7);
            UnloadingTask t3 = new UnloadingTask(3, 6, 2);
            UnloadingTask t4 = new UnloadingTask(4, 5, 9);

            LoadingTask l0 = new LoadingTask(0, new int[] { 5, 2, 0, 0, 3 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 0, 1, 0, 1, 3 });
            LoadingTask l2 = new LoadingTask(2, new int[] { 1, 0, 7, 1, 3 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1, t2, t3, t4 }, new LoadingTask[] { l0, l1, l2 });
            this.Plan = plan;
            int[,] scheduleUnloading = new int[5, 4];

            scheduleUnloading = new int[,]{    { 0, 0, 0, 0},
                                                { 0, 0, 10, 13},
                                                { 0, 0, 0, 0},
                                                { 1, 1, 6, 8},
                                                { 1, 1, 8, 17} };

            //int resfail = CheckIfDemandMet(scheduleUnloading, 0, 3, new int[] { 0, 0, 0 }, new int[] { 0, 1, 0, 1, 1 });
            //int respass =  sched.CheckIfDemandMet(scheduleUnloading, 1, 3, new int[] { 0, 0, 0 }, new int[] { 0, 1, 0, 1, 1 });
            // int resfail2 = sched.CheckIfDemandMet(scheduleUnloading, 2, 3, new int[] { 0, 0, 0 }, new int[] { 0, 1, 0, 1, 1 });

            int[] res = { 0, 0, 0, 0, 0 }; //sched.ScheduleOneLoading( 1, new int[] { 0 }, new int[] { 13, 17 }, respass);


            Assert.AreEqual(0, res[0]);

            if (res[1] == 0)
            {
                Assert.AreEqual(13, res[2]);
                Assert.AreEqual(18, res[3]);
            }
            else if (res[1] == 1)
            {
                Assert.AreEqual(17, res[2]);
                Assert.AreEqual(22, res[3]);
            }
            else
                Assert.IsTrue(false);

        }

        [Test]
        public void ScheduleTest()
        {
            UnloadingTask t0 = new UnloadingTask(0, 5, 6);
            UnloadingTask t1 = new UnloadingTask(1, 10, 3);
            UnloadingTask t2 = new UnloadingTask(2, 4, 7);
            UnloadingTask t3 = new UnloadingTask(3, 6, 2);
            UnloadingTask t4 = new UnloadingTask(4, 5, 9);

            LoadingTask l0 = new LoadingTask(0, new int[] { 5, 2, 0, 0, 3 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 0, 1, 0, 1, 3 });
            LoadingTask l2 = new LoadingTask(2, new int[] { 1, 0, 7, 1, 3 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1, t2, t3, t4 }, new LoadingTask[] { l0, l1, l2 });
            IComparer<UnloadingTask> comparer = new CompareTaskTime();
            FifoScheduler sched = new FifoScheduler(plan);
            Bee bee = sched.Schedule( );

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(bee.ScheduleUnloading[i, j] + " ");
                Console.WriteLine();
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(bee.ScheduleLoading[i, j] + " ");
                Console.WriteLine();
            }
        }

        [Test]
        public void ScheduleTest2()
        {
            ParametersValues.Instance.NumberOfIterations = 40;
            ParametersValues.Instance.MaxStorageCapacity = 100;
            ParametersValues.Instance.NumberOfWorkers = 2;
            ParametersValues.Instance.NumberOfInboundDocks = 2;
            ParametersValues.Instance.NumberOfOutboundDocks = 2;
            ParametersValues.Instance.NumberOfInboundTrucks = 2;
            ParametersValues.Instance.NumberOfOutboundTrucks = 2;
            ParametersValues.Instance.ScoutBeesNumber = 40;
            ParametersValues.Instance.SelectedRegionsNumber = 20;
            ParametersValues.Instance.EliteRegionsNumber = 10;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 5;
            ParametersValues.Instance.EliteRegionBeesNumber = 15;
            ParametersValues.Instance.TimePerProductUnit = 1;

            UnloadingTask t0 = new UnloadingTask(0, 5, 6);
            UnloadingTask t1 = new UnloadingTask(1, 10, 3);

            LoadingTask l0 = new LoadingTask(0, new int[] { 5, 2 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 0, 1 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1 }, new LoadingTask[] { l0, l1});
            IComparer<UnloadingTask> comparer = new CompareTaskTime();
            FifoScheduler sched = new FifoScheduler(plan);
            Bee bee = sched.Schedule();
            Console.WriteLine(bee.TimeOfWork);
            Console.WriteLine();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(bee.ScheduleUnloading[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(bee.ScheduleLoading[i, j] + " ");
                Console.WriteLine();
            }
        }
    }
}