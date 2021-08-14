using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossDock.Generators;
using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Models;

namespace CrossDock.Generators.Tests
{
    [TestClass()]
    public class TestDataGeneratorTests
    {
        [TestMethod()]
        public void GenerateTransportationPlanTest()
        {
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            TransportationPlan transportationPlan = testDataGenerator.GenerateTransportationPlan(50, 25, 20);
            for (int i = 0; i < transportationPlan.ArrivalTimes.Length; i++)
                Console.WriteLine(transportationPlan.ArrivalTimes[i]);
        }
    }
}