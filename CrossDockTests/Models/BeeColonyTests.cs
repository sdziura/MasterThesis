using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossDock.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Parameters;
using System.Collections;
using CrossDock.Schedulers;
using CrossDock.NeighborhoodSearch;

namespace CrossDock.Models.Tests
{
    [TestClass()]
    public class BeeColonyTests
    {
        [TestMethod()]
        public void BeeColonyConstructorsTest()
        {
            ParametersValues.Instance.MaxStorageCapacity = 1;
            ParametersValues.Instance.NumberOfWorkers = 2;
            ParametersValues.Instance.NumberOfInboundDocks = 2;
            ParametersValues.Instance.NumberOfOutboundDocks = 1;
            ParametersValues.Instance.NumberOfInboundTrucks = 5;
            ParametersValues.Instance.NumberOfOutboundTrucks = 3;
            ParametersValues.Instance.ScoutBeesNumber = 10;
            ParametersValues.Instance.SelectedRegionsNumber = 1;
            ParametersValues.Instance.EliteRegionsNumber = 1;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 1;
            ParametersValues.Instance.EliteRegionBeesNumber = 1;
            ParametersValues.Instance.TimePerProductUnit = 1;
            ParametersValues.Instance.TriesToSchedulePreError = 10;

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

            Console.WriteLine("\n----------------------\nThis one should FAIL: \n");
            BeeColony beeColony = new BeeColony(plan, new NeighborhoodSearchWorker(), comparer, new CompareTaskRandom<LoadingTask>());

            Console.WriteLine("\n----------------------\nThis one should PASS: \n");
            Console.WriteLine("Example bee: \n");
            ParametersValues.Instance.MaxStorageCapacity = 27;
            beeColony = new BeeColony(plan, new NeighborhoodSearchWorker(), comparer, new CompareTaskRandom<LoadingTask>());
            beeColony.Colony[6].PrintSchedule();

            Console.WriteLine("\nResults: \n");
            for(int i = 0; i < ParametersValues.Instance.ScoutBeesNumber; i++)
            {
                Console.WriteLine(beeColony.Colony[i].TimeOfWork + " ");
            }


        }

        [TestMethod()]
        public void BeeColonyAllGenerationsTest()
        {
            ParametersValues.Instance.MaxStorageCapacity = 100;
            ParametersValues.Instance.NumberOfWorkers = 2;
            ParametersValues.Instance.NumberOfInboundDocks = 2;
            ParametersValues.Instance.NumberOfOutboundDocks = 1;
            ParametersValues.Instance.NumberOfInboundTrucks = 5;
            ParametersValues.Instance.NumberOfOutboundTrucks = 3;
            ParametersValues.Instance.ScoutBeesNumber = 20;
            ParametersValues.Instance.SelectedRegionsNumber = 10;
            ParametersValues.Instance.EliteRegionsNumber = 5;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 5;
            ParametersValues.Instance.EliteRegionBeesNumber = 10;
            ParametersValues.Instance.TimePerProductUnit = 1;
            ParametersValues.Instance.TriesToSchedulePreError = 10;

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
            BeeColony beeColony = new BeeColony(plan, new NeighborhoodSearchWorker(), comparer, new CompareTaskRandom<LoadingTask>());

            Console.WriteLine("Before checking neighborhood: ");
            Console.WriteLine(beeColony.BestBee.TimeOfWork);
            Console.WriteLine("After checking neighborhood: ");
            beeColony.AllGenerations();
            Console.WriteLine( beeColony.BestBee.TimeOfWork);
            beeColony.BestBee.PrintSchedule();

            DepthFirstScheduler depth = new DepthFirstScheduler(plan);
            Bee bee = depth.Schedule();
            Console.WriteLine();
            Console.WriteLine(bee.TimeOfWork);
            bee.PrintSchedule();
        }

        [TestMethod()]
        public void BeeColonyTest2()
        {

            ParametersValues.Instance.MaxStorageCapacity = 100;
            ParametersValues.Instance.NumberOfWorkers = 1;
            ParametersValues.Instance.NumberOfInboundDocks = 1;
            ParametersValues.Instance.NumberOfOutboundDocks = 1;
            ParametersValues.Instance.NumberOfInboundTrucks = 5;
            ParametersValues.Instance.NumberOfOutboundTrucks = 5;
            ParametersValues.Instance.ScoutBeesNumber = 10;
            ParametersValues.Instance.SelectedRegionsNumber = 5;
            ParametersValues.Instance.EliteRegionsNumber = 3;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 5;
            ParametersValues.Instance.EliteRegionBeesNumber = 10;
            ParametersValues.Instance.TimePerProductUnit = 1;
            ParametersValues.Instance.TriesToSchedulePreError = 5;

            UnloadingTask t0 = new UnloadingTask(0, 5, 31);
            UnloadingTask t1 = new UnloadingTask(1, 25, 28);
            UnloadingTask t2 = new UnloadingTask(2, 18, 10);
            UnloadingTask t3 = new UnloadingTask(3, 19, 27);
            UnloadingTask t4 = new UnloadingTask(4, 12, 13);

            LoadingTask l0 = new LoadingTask(0, new int[] { 4,   17,  0 ,  8 ,  0 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 5,   0 ,  0 ,  0 ,  0 });
            LoadingTask l2 = new LoadingTask(2, new int[] { 5,   11,  10,  19,  13 });
            LoadingTask l3 = new LoadingTask(3, new int[] { 11,  0 ,  0 ,  0 ,  0 });
            LoadingTask l4 = new LoadingTask(4, new int[] { 6,   0 ,  0 ,  0 ,  0 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1, t2, t3, t4 }, new LoadingTask[] { l0, l1, l2, l3, l4 });

            IComparer<UnloadingTask> comparer = new CompareTaskTime();
            BeeColony beeColony = new BeeColony(plan, new NeighborhoodSearchWorker(), comparer, new CompareTaskRandom<LoadingTask>());
            
            Console.WriteLine("Before checking neighborhood: ");
            Console.WriteLine(beeColony.BestBee.TimeOfWork);
            Console.WriteLine("After checking neighborhood: ");
            beeColony.AllGenerations();
            Console.WriteLine(beeColony.BestBee.TimeOfWork);
            beeColony.BestBee.PrintSchedule();

            DepthFirstScheduler depth = new DepthFirstScheduler(plan);
            Bee bee = depth.Schedule();
            Console.WriteLine();
            Console.WriteLine(bee.TimeOfWork);
            bee.PrintSchedule();

        }

        [TestMethod()]
        public void BeeColonyTest3()
        {

            ParametersValues.Instance.MaxStorageCapacity = 100;
            ParametersValues.Instance.NumberOfWorkers = 1;
            ParametersValues.Instance.NumberOfInboundDocks = 1;
            ParametersValues.Instance.NumberOfOutboundDocks = 1;
            ParametersValues.Instance.NumberOfInboundTrucks = 5;
            ParametersValues.Instance.NumberOfOutboundTrucks = 5;
            ParametersValues.Instance.ScoutBeesNumber = 10;
            ParametersValues.Instance.SelectedRegionsNumber = 5;
            ParametersValues.Instance.EliteRegionsNumber = 3;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 5;
            ParametersValues.Instance.EliteRegionBeesNumber = 10;
            ParametersValues.Instance.TimePerProductUnit = 1;
            ParametersValues.Instance.TriesToSchedulePreError = 5;

            UnloadingTask t0 = new UnloadingTask(0, 5, 31);
            UnloadingTask t1 = new UnloadingTask(1, 25, 28);
            UnloadingTask t2 = new UnloadingTask(2, 18, 10);
            UnloadingTask t3 = new UnloadingTask(3, 19, 27);
            UnloadingTask t4 = new UnloadingTask(4, 12, 13);

            LoadingTask l0 = new LoadingTask(0, new int[] { 4, 17, 0, 8, 0 });
            LoadingTask l1 = new LoadingTask(1, new int[] { 5, 0, 0, 0, 0 });
            LoadingTask l2 = new LoadingTask(2, new int[] { 5, 11, 10, 19, 13 });
            LoadingTask l3 = new LoadingTask(3, new int[] { 11, 0, 0, 0, 0 });
            LoadingTask l4 = new LoadingTask(4, new int[] { 6, 0, 0, 0, 0 });

            TransportationPlan plan = new TransportationPlan(new UnloadingTask[] { t0, t1, t2, t3, t4 }, new LoadingTask[] { l0, l1, l2, l3, l4 });
            IComparer<UnloadingTask> comparer = new CompareTaskTime();
            BeeColony beeColony = new BeeColony(plan, new NeighborhoodSearchWorker(), comparer, new CompareTaskRandom<LoadingTask>());

            Console.WriteLine("Before checking neighborhood: ");
            // Console.WriteLine(beeColony.BestBee.TimeOfWork);
            ////beeColony.BestBee.PrintSchedule();
            Console.WriteLine("After checking neighborhood: ");
            beeColony.NextIteration();
            Console.WriteLine(beeColony.BestBee.TimeOfWork);
            beeColony.BestBee.PrintSchedule();

        }
    }
}