using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleView
{
    public class Participant
    {
        public Competition? competition;
        public Equipment equipment;
        public Team team;

        public Participant(Equipment equipment)
        {
            this.equipment = equipment;
        }
    }

    public class Equipment
    {
        public Participant participant;
    }

    public class Team
    {
        public Participant participant;
    }
}
