using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
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

        public Dictionary<Section, SectionData> Positions { get => _positions; set => _positions = value;  }

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;

            _random = new(DateTime.UtcNow.Millisecond);

            _positions = new();
            foreach (Section section in track.Sections)
            {
                _positions.Add(section, new());
            }
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

        public Section[] GetStartPositions()
        {
            List<Section> result = new();
            foreach (var section in _positions.Keys)
            {
                if (section.SectionType == SectionType.StartGrid)
                {
                    result.Add(section);
                }
            }

            return result.ToArray();
        }

        public void SetStartingPositions()
        {
            Section[] startingPositions = GetStartPositions();
            if (startingPositions.Length * 2 < Participants.Count)
            {
                throw new ArgumentException("Not enough starting positions for that many participants!");
            }

            for (int s = 0, p = 0; p < Participants.Count; p++)
            {
                SectionData data = GetSectionData(startingPositions[s]);
                IParticipant participant = Participants[p];
                data.Left ??= participant;

                if (data.Left != participant)
                {
                    data.Right = participant;
                    Positions[startingPositions[s]] = data;
                    s++;
                }
                else
                {
                    Positions[startingPositions[s]] = data;
                }
            }
        }
    }
}
