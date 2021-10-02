using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrossDock.Models;

namespace CrossDock.Schedulers
{
    interface IScheduler
    {
        Bee Schedule();
        Bee Reschedule(Bee bee, int time);
        Bee RescheduleV2(Bee bee, int time);
        Bee DynamicReschedule(Bee bee, int time);

    }
}
