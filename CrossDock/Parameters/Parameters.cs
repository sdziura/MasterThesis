using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Parameters
{
    public sealed class ParametersValues
    {
        static ParametersValues _instance;

        private int _maxStorageCapacity;
        private int _numberOfWorkers;
        private int _numberOfInboundDocks;
        private int _numberOfOutboundDocks;
        private int _numberOfInboundTrucks;
        private int _numberOfOutboungTrucks;

        private int _scoutBeesNumber;
        private int _SelectedRegionsNumber;
        private int _eliteBeesNumber;
        private int _selectedRegionsBeesNumber;
        private int _eliteRegionBeesNumber;

        private ParametersValues() { }

        public static ParametersValues Instance
        {
            get { return _instance ?? (_instance = new ParametersValues()); }
            set { _instance = value; }
        }

        public int MaxStorageCapacity { get => _maxStorageCapacity; set => _maxStorageCapacity = value; }
        public int NumberOfWorkers { get => _numberOfWorkers; set => _numberOfWorkers = value; }
        public int NumberOfInboundDocks { get => _numberOfInboundDocks; set => _numberOfInboundDocks = value; }
        public int NumberOfOutboundDocks { get => _numberOfOutboundDocks; set => _numberOfOutboundDocks = value; }
        public int NumberOfInboundTrucks { get => _numberOfInboundTrucks; set => _numberOfInboundTrucks = value; }
        public int NumberOfOutboungTrucks { get => _numberOfOutboungTrucks; set => _numberOfOutboungTrucks = value; }
        public int ScoutBeesNumber { get => _scoutBeesNumber; set => _scoutBeesNumber = value; }
        public int SelectedRegionsNumber { get => _SelectedRegionsNumber; set => _SelectedRegionsNumber = value; }
        public int EliteBeesNumber { get => _eliteBeesNumber; set => _eliteBeesNumber = value; }
        public int SelectedRegionsBeesNumber { get => _selectedRegionsBeesNumber; set => _selectedRegionsBeesNumber = value; }
        public int EliteRegionBeesNumber { get => _eliteRegionBeesNumber; set => _eliteRegionBeesNumber = value; }
    }
}
