using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Models
{
    public class BeeColony
    {
        private Bee[] _colony;

        public BeeColony(Bee[] colony)
        {
            _colony = colony;
        }

        public Bee[] Colony { get => _colony; set => _colony = value; }
    }
}
