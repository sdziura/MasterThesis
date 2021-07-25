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
        INeighborhoodSearch _neighborhoodSearch;
        //IComparer _comparer;

        public BeeColony()
        {

        }

        public BeeColony(TransportationPlan plan, INeighborhoodSearch neighborhoodSearch, IComparer<UnloadingTask> unloadingComparer, IComparer<LoadingTask> loadingComparer)
        {
            _scheduler = new FifoScheduler(plan, unloadingComparer, loadingComparer);
            _colony = new Bee[ParametersValues.Instance.ScoutBeesNumber];
            _neighborhoodSearch = neighborhoodSearch;
            //_comparer = comparer;

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
                    _colony[i] = _scheduler.Schedule();
                } while (!_colony[i].CheckStorage());
            }
            Array.Sort(_colony, new CompareBee());
        }

        public BeeColony(Bee[] colony)
        {
            _colony = colony;
        }

        public void NextIteration()
        {
            IComparer beeComparer = new CompareBee();
            for(int scoutID = 0; scoutID < ParametersValues.Instance.ScoutBeesNumber; scoutID++ )
            {
                Bee newBee = new Bee();
                if (scoutID < ParametersValues.Instance.EliteRegionsNumber)
                {
                    Bee[] neighborBees = new Bee[ParametersValues.Instance.EliteRegionBeesNumber];
                    for (int i = 0; i < ParametersValues.Instance.EliteRegionBeesNumber; i++)
                    {
                        int tries = 0;
                        do
                        {
                            if (tries++ > ParametersValues.Instance.TriesToSchedulePreError)
                            {
                                Console.WriteLine("Could not find neighbor elite nr " + i + " after " + (tries - 2) + " tries.\nStorage oveloaded over maximum " + ParametersValues.Instance.MaxStorageCapacity);
                                return;
                            }
                            neighborBees[i] = NeighborhoodSearch.SearchRegion(_colony[scoutID]);
                        } while (!neighborBees[i].CheckStorage());
                    }
                    Array.Sort(neighborBees, beeComparer);
                    newBee = neighborBees[0];
                }
                else if (scoutID < ParametersValues.Instance.SelectedRegionsNumber)
                {
                    Bee[] neighborBees = new Bee[ParametersValues.Instance.SelectedRegionsBeesNumber];
                    for (int i = 0; i < ParametersValues.Instance.SelectedRegionsBeesNumber; i++)
                    {
                        int tries = 0;
                        do
                        {
                            if (tries++ > ParametersValues.Instance.TriesToSchedulePreError)
                            {
                                Console.WriteLine("Could not find neighbor normal nr " + i + " after " + (tries - 2) + " tries.\nStorage oveloaded over maximum " + ParametersValues.Instance.MaxStorageCapacity);
                                return;
                            }
                            neighborBees[i] = NeighborhoodSearch.SearchRegion(_colony[scoutID]);
                        } while (!neighborBees[i].CheckStorage());
                    }
                    Array.Sort(neighborBees, beeComparer);
                    newBee = neighborBees[0];
                }
                else
                {
                    int tries = 0;
                    do
                    {
                        if (tries++ > ParametersValues.Instance.TriesToSchedulePreError)
                        {
                            Console.WriteLine("Could not find neighbor new bee " + " after " + (tries - 2) + " tries.\nStorage oveloaded over maximum " + ParametersValues.Instance.MaxStorageCapacity);
                            return;
                        }
                        newBee = _scheduler.Schedule();
                    } while (!newBee.CheckStorage());
                }
                if (newBee.TimeOfWork < _colony[scoutID].TimeOfWork)
                    _colony[scoutID] = newBee;
            }
            Array.Sort(_colony, beeComparer);
        }

        public void AllGenerations()
        {
            for(int i = 0; i < ParametersValues.Instance.NumberOfIterations; i++)
            {
                NextIteration();
            }
        }

        public Bee[] Colony { get => _colony; set => _colony = value; }
        public INeighborhoodSearch NeighborhoodSearch { get => _neighborhoodSearch; set => _neighborhoodSearch = value; }
        //public IComparer Comparer { get => _comparer; set => _comparer = value; }
        public Bee BestBee { get => Colony[0]; }
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
