using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleView
{
    public class Competition
    {
        public Track[] tracks;
        public Participant[] participants;

        public Competition(Participant[] participants, Track[] tracks)
        {
            if (participants.Length < 3)
            {
                throw new ArgumentException("Requires at least 3 participants");
            }

            if (tracks.Length < 2)
            {
                throw new ArgumentException("Requires at least 2 tracks");
            }

            this.participants = participants;
            this.tracks = tracks;
        }
    }
}
