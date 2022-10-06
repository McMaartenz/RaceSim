using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

using Model;

namespace Controller
{ 
    public class Race
    {
        public const int requiredRounds = 2;

        public delegate void DriversChangedEventHandler(object sender, DriversChangedEventArgs e);
        public event DriversChangedEventHandler DriversChanged;

        private System.Timers.Timer timer;

        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public Dictionary<Section, SectionData> Positions { get => _positions; set => _positions = value;  }

        private Dictionary<IParticipant, int> Rounds { get; set; }

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            timer = new(500); //TODO make 500
            timer.Elapsed += OnTimedEvent;

            Rounds = new(participants.Count);
            _random = new(DateTime.UtcNow.Millisecond);

            _positions = new();
            foreach (Section section in track.Sections)
            {
                _positions.Add(section, new());
            }

            Start();
        }

        public void Start()
        {
            timer.Start();
        }

        public void RemoveEvents()
        {
            DriversChanged = null;
            timer.Stop();
            timer.Elapsed -= OnTimedEvent;
        }

        public void OnTimedEvent(object? sender, EventArgs e)
        {
            Random rng = new();
            foreach (IParticipant participant in Participants)
            {
                IEquipment equipment = participant.Equipment;
                if (equipment.IsBroken)
                {
                    int repairChance = 6;

                    if (rng.Next(repairChance) == 0)
                    {
                        equipment.IsBroken = false;
                    }
                }
                else
                {
                    int breakingChance = 30 + (equipment.Quality / 100) * 2;

                    if (rng.Next(breakingChance) == 0)
                    {
                        equipment.IsBroken = true;
                    }
                }
            }

            foreach (Section section in Track.Sections)
            {
                SectionData sData = GetSectionData(section);

                if (sData.Left != null && !sData.Left.Equipment.IsBroken)
                {
                    sData.DistanceLeft += sData.Left.Equipment.GetSpeed();
                    if (sData.DistanceLeft > Section.sectionLength)
                    {
                        IParticipant participant = sData.Left;

                        LinkedListNode<Section> next = Track.Sections.Find(section).Next ?? Track.Sections.First;
                        Section nextSection = next.Value;
                        SectionData nextSData = GetSectionData(nextSection);

                        if (nextSData.Left == null)
                        {
                            nextSData.Left = participant;
                            sData.Left = null;
                            sData.DistanceLeft = 0;

                            if (section.SectionType == SectionType.Finish)
                            {
                                if (Rounds.ContainsKey(participant))
                                {
                                    Rounds[participant]++;
                                    if (Rounds[participant] >= requiredRounds)
                                    {
                                        nextSData.Left = null;
                                    }
                                }
                                else
                                {
                                    Rounds.Add(participant, 0);
                                }
                            }
                        }
                    }
                }

                if (sData.Right != null && !sData.Right.Equipment.IsBroken)
                {
                    sData.DistanceRight += sData.Right.Equipment.GetSpeed();
                    if (sData.DistanceRight > Section.sectionLength)
                    {
                        IParticipant participant = sData.Right;

                        LinkedListNode<Section> next = Track.Sections.Find(section).Next ?? Track.Sections.First;
                        Section nextSection = next.Value;
                        SectionData nextSData = GetSectionData(nextSection);

                        if (nextSData.Right == null)
                        {
                            nextSData.Right = participant;
                            sData.Right = null;
                            sData.DistanceRight = 0;

                            if (section.SectionType == SectionType.Finish)
                            {
                                if (Rounds.ContainsKey(participant))
                                {
                                    Rounds[participant]++;
                                    if (Rounds[participant] >= requiredRounds)
                                    {
                                        nextSData.Right = null;
                                    }
                                }
                                else
                                {
                                    Rounds.Add(participant, 0);
                                }
                            }
                        }
                    }
                }
            }
            DriversChanged?.Invoke(sender, new(Track));
            
            StringBuilder sb = new("Rounds:");

            bool everyoneDone = true;
            foreach (IParticipant participant in Participants)
            {
                if (Rounds.TryGetValue(participant, out int rounds))
                {
                    sb.Append($" {participant}: {rounds},");
                    if (rounds < requiredRounds)
                    {
                        everyoneDone = false;
                    }
                }
            }
            
            Console.Title = sb.Remove(sb.Length - 1, 1).ToString();
        
            if (everyoneDone && Participants.Count == Rounds.Count)
            {
                Data.NextRace();
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
