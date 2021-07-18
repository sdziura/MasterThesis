using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrossDock.Parameters;
using CrossDock.Schedulers;
using CrossDock.NeighborhoodSearch;

namespace CrossDock.Models
{
    public class BeeColony
    {
        private Bee[] _colony;
        private IScheduler _scheduler;

        public BeeColony()
        {

        }

        public BeeColony(TransportationPlan plan, IComparer comparer)
        {
            _scheduler = new FifoScheduler(plan);
            _colony = new Bee[ParametersValues.Instance.ScoutBeesNumber];

            for(int i = 0; i < ParametersValues.Instance.ScoutBeesNumber; i++)
            {
                int tries = 0;
                do
                {
                    if(tries++ > ParametersValues.Instance.TriesToSchedulePreError)
                    {
                        Console.WriteLine("Could not find bee nr " + i + " after " + (tries - 2) + " tries.\nStorage oveloaded over maximum " + ParametersValues.Instance.MaxStorageCapacity );
                        return;
                    }
                    _colony[i] = _scheduler.Schedule(comparer);
                } while (!_colony[i].CheckStorage());
            }
            Array.Sort(_colony, new CompareBee());
        }

        public BeeColony(Bee[] colony)
        {
            _colony = colony;
        }

        public void NextIteration(INeighborhoodSearch neighborhoodSearch, IComparer comparer)
        {
            IComparer beeComparer = new CompareBee();
            for(int scoutID = 0; scoutID < ParametersValues.Instance.ScoutBeesNumber; scoutID++ )
            {
                Bee newBee = new Bee();
                if (scoutID < ParametersValues.Instance.EliteRegionsNumber)
                {
                    Bee[] neighborBees = new Bee[ParametersValues.Instance.EliteRegionBeesNumber];
                    for (int i = 0; i < ParametersValues.Instance.EliteRegionBeesNumber; i++)
                        do neighborBees[i] = neighborhoodSearch.SearchRegion(_colony[scoutID]); while (!neighborBees[i].CheckStorage());
                    Array.Sort(neighborBees, beeComparer);
                    newBee = neighborBees[0];
                }
                else if (scoutID < ParametersValues.Instance.SelectedRegionsNumber)
                {
                    Bee[] neighborBees = new Bee[ParametersValues.Instance.SelectedRegionsBeesNumber];
                    for (int i = 0; i < ParametersValues.Instance.SelectedRegionsBeesNumber; i++)
                        do neighborBees[i] = neighborhoodSearch.SearchRegion(_colony[scoutID]); while (!neighborBees[i].CheckStorage());
                    Array.Sort(neighborBees, beeComparer);
                    newBee = neighborBees[0];
                }
                else
                {
                    do newBee = _scheduler.Schedule(comparer); while (!newBee.CheckStorage());
                }
                if (newBee.TimeOfWork < _colony[scoutID].TimeOfWork)
                    _colony[scoutID] = newBee;
            }
            Array.Sort(_colony, beeComparer);
        }

        public Bee[] Colony { get => _colony; set => _colony = value; }
    }

    public class CompareBee : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            Bee a = (Bee)x;
            Bee b = (Bee)y;
            if (a.TimeOfWork < b.TimeOfWork)
                return -1;
            else if (a.TimeOfWork > b.TimeOfWork)
                return 1;
            else
                return 0;
        }
    }
}
