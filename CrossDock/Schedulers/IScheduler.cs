using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Models;

namespace CrossDock.Schedulers
{
    interface IScheduler
    {
        Bee Schedule(TransportationPlan plan);
        Bee Reschedule(TransportationPlan plan, Bee bee, int time);
    }
}
