using System;
using System.Collections.Generic;
using System.Text;
using CrossDock.Models;

namespace CrossDock.NeighborhoodSearch
{
    public interface INeighborhoodSearch
    {
        //Bee SearchEliteRegion(Bee bee);
        Bee SearchRegion(Bee bee);
    }
}
