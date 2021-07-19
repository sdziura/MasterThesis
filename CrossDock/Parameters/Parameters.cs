using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Parameters
{
    public sealed class ParametersValues
    {
        static ParametersValues _instance;

        // Cross dock center parameters 
        private int _maxStorageCapacity;
        private int _numberOfWorkers;
        private int _numberOfInboundDocks;
        private int _numberOfOutboundDocks;
        private int _numberOfInboundTrucks;
        private int _numberOfOutboundTrucks;
        private int _timePerProductUnit;

        // All bees looking for initial solutions
        private int _scoutBeesNumber;
        // Number of initial solutions in each category (selected regions + elite bees + bees to change = scout bees)
        private int _SelectedRegionsNumber;
        private int _eliteRegionsNumber;
        // Number of bees looking for neighorhood 
        private int _selectedRegionsBeesNumber;
        private int _eliteRegionBeesNumber;

        private int _numberOfIterations;

        private int _triesToSchedulePreError;

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
        public int NumberOfOutboundTrucks { get => _numberOfOutboundTrucks; set => _numberOfOutboundTrucks = value; }
        public int ScoutBeesNumber { get => _scoutBeesNumber; set => _scoutBeesNumber = value; }
        public int SelectedRegionsNumber { get => _SelectedRegionsNumber; set => _SelectedRegionsNumber = value; }
        public int EliteRegionsNumber { get => _eliteRegionsNumber; set => _eliteRegionsNumber = value; }
        public int SelectedRegionsBeesNumber { get => _selectedRegionsBeesNumber; set => _selectedRegionsBeesNumber = value; }
        public int EliteRegionBeesNumber { get => _eliteRegionBeesNumber; set => _eliteRegionBeesNumber = value; }
        public int TimePerProductUnit { get => _timePerProductUnit; set => _timePerProductUnit = value; }
        public int TriesToSchedulePreError { get => _triesToSchedulePreError; set => _triesToSchedulePreError = value; }
        public int NumberOfIterations { get => _numberOfIterations; set => _numberOfIterations = value; }
    }
}
