using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class DataContext : INotifyPropertyChanged
    {
        public string TrackName { get; set; }
        public List<ParticipantInfo> Participants { get; set; }

        public List<FinishInfo> FinishedParticipants { get; set; }

        public Queue<TrackInfo> UpcomingTracks { get; set; }

        public string StartTimeCurrentRace { get; set; }

        public List<ParticipantEquipmentInfo> ParticipantAndEquipmentInfo { get; set; }

        public record ParticipantInfo(TeamColor Color, string Name, int Rounds, int Sections, int Distance, bool Broken);
        public record ParticipantEquipmentInfo(TeamColor Color, string Name, int Quality, int Performance, int Speed);
        public record FinishInfo(TeamColor Color, string Name, int Rounds, string Time);
        public record TrackInfo(string Name, int Sections);

        public DataContext()
        {
            TrackName = "Untitled track";
            Participants = new();
            StartTimeCurrentRace = DateTime.Now.ToString();
            FinishedParticipants = new();
            ParticipantAndEquipmentInfo = new();

            Data.RaceChanged += (sender, e) =>
            {
                TrackName = e.race.Track.Name;
                StartTimeCurrentRace = $"Start time: {e.race.StartTime}";
                ParticipantAndEquipmentInfo = Data.Competition.Participants.Select(x =>
                {
                    IEquipment e = x.Equipment;
                    return new ParticipantEquipmentInfo(x.TeamColor, x.Name, e.Quality, e.Performance, e.Speed);
                }).OrderBy(x => x.Name).ToList();

                e.race.DriversChanged += (sender, ev) =>
                {
                    Dictionary<IParticipant, (int sectionN, int distance)> sectionCount = new();
                    int sectionN = 0;
                    foreach (Section section in e.race.Track.Sections)
                    {
                        SectionData data = e.race.GetSectionData(section);
                        if (data.Left is not null)
                        {
                            sectionCount.Add(data.Left, (sectionN, data.DistanceLeft));
                        }

                        if (data.Right is not null)
                        {
                            sectionCount.Add(data.Right, (sectionN, data.DistanceRight));
                        }

                        sectionN++;
                    }

                    Dictionary<IParticipant, int> participantRounds = new();
                    e.race.Participants.ForEach(p =>
                    {
                        bool ok = e.race.Rounds.TryGetValue(p, out int rounds);
                        participantRounds.Add(p, ok ? rounds : 0);
                    });

                    Dictionary<IParticipant, (int sectionN, int distance, int rounds)> participantCombinedData = new();
                    e.race.Participants.ForEach(p =>
                    {
                        bool ok = sectionCount.TryGetValue(p, out (int sectionN, int distance) output);
                        if (!ok) { output.sectionN = 0; output.distance = 0; }
                        participantCombinedData.Add(p, (output.sectionN, output.distance, participantRounds[p]));
                    });

                    Participants = participantCombinedData.OrderByDescending(x => x.Value.rounds)
                        .OrderByDescending(x => x.Value.distance)
                        .OrderByDescending(x => x.Value.sectionN)
                        .Select(x => new ParticipantInfo
                        (
                            Color: x.Key.TeamColor,
                            Name: x.Key.Name,
                            Rounds: x.Value.rounds,
                            Sections: x.Value.sectionN,
                            Distance: x.Value.distance,
                            Broken: x.Key.Equipment.IsBroken
                        )).ToList();

                    FinishedParticipants = e.race.FinishTime.Select(x => new FinishInfo
                    (
                        Color: x.Key.TeamColor,
                        Name: x.Key.Name,
                        Rounds: participantRounds[x.Key],
                        Time: x.Value.Subtract(e.race.StartTime).ToString()
                    )).ToList();

                    PropertyChanged?.Invoke(this, new(""));
                };

                UpcomingTracks = new Queue<TrackInfo>(Data.Competition.Tracks.Select(x => new TrackInfo(x.Name, x.Sections.Count)));
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
