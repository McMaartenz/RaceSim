using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;

            _random = new(DateTime.UtcNow.Millisecond);
        }

        public void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                IEquipment equipment = participant.Equipment;
                equipment.Quality = _random.Next(100);
                equipment.Performance = _random.Next(100);
            }
        }

        public SectionData GetSectionData(Section section)
        {
            if (!_positions.ContainsKey(section))
            {
                _positions[section] = new();
            }

            return _positions[section];
        }
    }
}
