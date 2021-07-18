using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrossDock.Parameters;
using CrossDock.Schedulers;

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
        }

        public BeeColony(Bee[] colony)
        {
            _colony = colony;
        }

        public Bee[] Colony { get => _colony; set => _colony = value; }
    }
}
