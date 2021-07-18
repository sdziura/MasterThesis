using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossDock.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Parameters;
using System.Collections;
using CrossDock.Schedulers;

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
            ParametersValues.Instance.EliteBeesNumber = 1;
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
            IComparer comparer = new CompareTaskTime();

            Console.WriteLine("\n----------------------\nThis one should FAIL: \n");
            BeeColony beeColony = new BeeColony(plan, comparer);

            Console.WriteLine("\n----------------------\nThis one should PASS: \n");
            Console.WriteLine("Example bee: \n");
            ParametersValues.Instance.MaxStorageCapacity = 27;
            beeColony = new BeeColony(plan, comparer);
            beeColony.Colony[6].PrintSchedule();

            Console.WriteLine("\nResults: \n");
            for(int i = 0; i < ParametersValues.Instance.ScoutBeesNumber; i++)
            {
                Console.WriteLine(beeColony.Colony[i].TimeOfWork + " ");
            }


        }

        [TestMethod()]
        public void BeeColonyTest1()
        {

        }

        [TestMethod()]
        public void BeeColonyTest2()
        {

        }
    }
}