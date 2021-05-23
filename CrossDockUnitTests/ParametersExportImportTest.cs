using NUnit.Framework;
using CrossDock.Parameters;
using System.IO;
using System;

namespace CrossDockUnitTests
{
    public class ParameteresExportImportTests
    {
        [SetUp]
        public void Setup()
        {

            ParametersValues.Instance.MaxStorageCapacity = 1;
            ParametersValues.Instance.NumberOfWorkers = 1;
            ParametersValues.Instance.NumberOfInboundDocks = 1;
            ParametersValues.Instance.NumberOfOutboundDocks = 1;
            ParametersValues.Instance.NumberOfInboundTrucks = 1;
            ParametersValues.Instance.NumberOfOutboungTrucks = 1;
            ParametersValues.Instance.ScoutBeesNumber = 1;
            ParametersValues.Instance.SelectedRegionsNumber = 1;
            ParametersValues.Instance.EliteBeesNumber = 1;
            ParametersValues.Instance.SelectedRegionsBeesNumber = 1;
            ParametersValues.Instance.EliteRegionBeesNumber = 1;            
        }

        [Test]
        public void Test1()
        {
            var parImEx = new ParametersExportImport();
            Assert.AreEqual(true, parImEx.ExportParameters("test.json"));       
        }

        [Test]
        public void Test2()
        {
            var parImEx = new ParametersExportImport();
            string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_files", "parameters_settings");
            
            Assert.AreEqual(true, parImEx.ImportParameters(baseDirectory + @"\test.json"));
            Assert.AreEqual(1, ParametersValues.Instance.MaxStorageCapacity);
        }
    }
}