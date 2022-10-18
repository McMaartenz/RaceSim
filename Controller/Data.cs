using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Model;

using static Model.SectionType;

namespace Controller
{
    public static class Data
    {
        public delegate void RaceChangedEventHandler(object? sender, RaceChangedEventArgs e);
        public static event RaceChangedEventHandler RaceChanged;

        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }
        
        public static void GenerateParticipants()
        {
            List<IParticipant> participants = new(); // TODO: change speed to 100
            participants.Add(new Skater("Alpha", 0, new Skates(200, 125, 5, false), TeamColor.Red));
            participants.Add(new Skater("Bravo", 0, new Skates(200, 125, 5, false), TeamColor.Blue));
            participants.Add(new Skater("Charlie", 0, new Skates(200, 125, 5, false), TeamColor.Grey));

            Competition.Participants = participants;
        }

        public static void GenerateTracks()
        {
            SectionType[] zwolleSections =
            {
                StartGrid, // 0 0
                StartGrid, // 1 0
                Finish, // 2 0
                RightCorner, // 3 0
                Straight, // 3 1
                RightCorner, // 3 2
                Straight, // 2 2
                Straight, // 1 2
                Straight, // 0 2
                RightCorner, // -1 2
                Straight, // -1 1
                RightCorner, // -1 0
            };

            SectionType[] enschedeSections =
            {
                StartGrid, // 0 0
                StartGrid, // 1 0
                Finish, // 2 0
                RightCorner, // 3 0
                Straight, // 3 1
                Straight, // 3 2
                RightCorner, // 3 3
                Straight, // 2 3
                Straight, // 1 3
                Straight, // 0 3
                RightCorner, // -1 3
                Straight, // -1 2
                Straight, // -1 1
                RightCorner, // -1 0
            };

            Queue<Track> tracks = new();
            tracks.Enqueue(new Track("Zwolle", zwolleSections));
            tracks.Enqueue(new Track("Enschede", enschedeSections));

            Competition.Tracks = tracks;
        }

        public static void Initialize()
        {
            Competition = new();

            GenerateParticipants();
            GenerateTracks();
        }

        public static void NextRace()
        {
            Track? nextTrack = Competition.NextTrack();
            if (nextTrack is not null)
            {
                CurrentRace?.RemoveEvents();
                CurrentRace = new(nextTrack, Competition.Participants);
                CurrentRace.SetStartingPositions();

                // send current race changed event
                RaceChanged?.Invoke(default, new(CurrentRace));
            }
        }
    }
}
