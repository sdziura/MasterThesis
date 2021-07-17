using NUnit.Framework;
using CrossDock.Parameters;
using CrossDock.NeighborhoodSearch;
using System.IO;
using System;
using CrossDock.Schedulers;
using CrossDock.Models;
using System.Collections;

namespace CrossDockUnitTests
{
    class NeighborhoodSearchTest
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
            ParametersValues.Instance.EliteBeesNumber = 1;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 1;
            ParametersValues.Instance.EliteRegionBeesNumber = 1;
            ParametersValues.Instance.TimePerProductUnit = 1;
        }

        [Test]
        public void SearchRegionTest()
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
            IComparer comparer = new CompareTaskTime();
            FifoScheduler sched = new FifoScheduler(plan);
            Bee bee = sched.Schedule(comparer);

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

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("After neighorhood search:"); 
            Console.WriteLine();

            NeighborhoodSearchWorker neigherhood = new NeighborhoodSearchWorker();

            bee = neigherhood.SearchRegion(bee, plan);

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


        }

    }
}
