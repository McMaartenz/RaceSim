using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class RaceChangedEventArgs : EventArgs
    {
        public Race race;

        public RaceChangedEventArgs(Race race)
        {
            this.race = race;
        }
    }
}
