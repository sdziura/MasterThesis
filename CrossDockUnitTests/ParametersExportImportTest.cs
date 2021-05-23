using NUnit.Framework;
using CrossDock.Parameteres;

namespace CrossDockUnitTests
{
    public class ParameteresExportImportTests
    {
        [SetUp]
        public void Setup()
        {

            Parameters.Instance.MaxStorageCapacity = 1;
            Parameters.Instance.NumberOfWorkers = 1;
            Parameters.Instance.NumberOfInboundDocks = 1;
            Parameters.Instance.NumberOfOutboundDocks = 1;
            Parameters.Instance.NumberOfInboundTrucks = 1;
            Parameters.Instance.NumberOfOutboungTrucks = 1;
            Parameters.Instance.ScoutBeesNumber = 1;
            Parameters.Instance.SelectedRegionsNumber = 1;
            Parameters.Instance.EliteBeesNumber = 1;
            Parameters.Instance.SelectedRegionsBeesNumber = 1;
            Parameters.Instance.EliteRegionBeesNumber = 1;

            
            
        }

        [Test]
        public void Test1()
        {
            var parImEx = new ParametersExportImport();
            Assert.AreEqual(true, parImEx.ExportParameters("test.txt"));       
        }
    }
}