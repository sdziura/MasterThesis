using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrossDock.Models;

namespace CrossDock.Schedulers
{
    interface IScheduler
    {
        Bee Schedule(TransportationPlan plan, IComparer comparer);
        Bee Reschedule(TransportationPlan plan, Bee bee, int time);
    }
}
