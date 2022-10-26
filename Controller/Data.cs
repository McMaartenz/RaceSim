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
            List<IParticipant> participants = new(); // TODO: change speed to 200
            participants.Add(new Skater("Alpha", 0, new Skates(200, 125, 5, false), TeamColor.Red));
            participants.Add(new Skater("Bravo", 0, new Skates(200, 125, 5, false), TeamColor.Green));
            participants.Add(new Skater("Charlie", 0, new Skates(200, 125, 5, false), TeamColor.Blue));
            participants.Add(new Skater("Delta", 0, new Skates(200, 125, 5, false), TeamColor.Yellow));

            Competition.Participants = participants;
        }

        public static void GenerateTracks()
        {
            #region Tracks
            SectionType[] izoliraniSections =
            {                // O X Y
                Straight,    // E 1 0
                Straight,    // E 2 0
                Straight,    // E 3 0
                Straight,    // E 4 0
                Straight,    // E 5 0
                Straight,    // E 6 0
                Straight,    // E 7 0
                RightCorner, // S 8 0
                Straight,    // S 8 1
                RightCorner, // W 8 2
                RightCorner, // N 7 2
                LeftCorner,  // W 7 1
                LeftCorner,  // S 6 1
                Straight,    // S 6 2
                Straight,    // S 6 3
                StartGrid,   // S 6 4
                StartGrid,   // S 6 5
                StartGrid,   // S 6 6
                Finish,      // S 6 7
                LeftCorner,  // E 6 8
                LeftCorner,  // N 7 8
                RightCorner, // E 7 7
                RightCorner, // S 8 7
                LeftCorner,  // E 8 8
                Straight,    // E 9 8
                LeftCorner,  // N A 8
                Straight,    // N A 7
                Straight,    // N A 6
                Straight,    // N A 5
                Straight,    // N A 4
                LeftCorner,  // W A 3
                Straight,    // W 9 3
                Straight,    // W 8 3
                Straight,    // W 7 3
                Straight,    // W 6 3
                Straight,    // W 5 3
                Straight,    // W 4 3
                RightCorner, // N 3 3
                RightCorner, // E 3 2
                RightCorner, // S 4 2
                Straight,    // S 4 3
                Straight,    // S 4 4
                RightCorner, // W 4 5
                LeftCorner,  // S 3 5
                Straight,    // S 3 6
                Straight,    // S 3 7
                LeftCorner,  // E 3 8
                LeftCorner,  // N 4 8
                LeftCorner,  // W 4 7
                Straight,    // W 3 7
                Straight,    // W 2 7
                Straight,    // W 1 7
                RightCorner, // W 0 7
                Straight,    // W 0 6
                Straight,    // W 0 5
                Straight,    // W 0 4
                Straight,    // W 0 3
                Straight,    // W 0 2
                Straight,    // W 0 1
                RightCorner, // W 0 0
            };

            SectionType[] zwolleSections =
            {                // O X Y
                StartGrid,   // E 1 0
                StartGrid,   // E 2 0
                Finish,      // E 3 0
                RightCorner, // S 4 0
                Straight,    // S 4 1
                RightCorner, // W 4 2
                Straight,    // W 3 2
                Straight,    // W 2 2
                Straight,    // W 1 2
                RightCorner, // N 0 2
                Straight,    // N 0 1
                RightCorner, // E 0 0
            };

            SectionType[] enschedeSections =
            {                // O X Y
                StartGrid,   // E 1 0
                StartGrid,   // E 2 0
                Finish,      // E 3 0
                RightCorner, // S 4 0
                Straight,    // S 4 1
                Straight,    // S 4 2
                RightCorner, // W 4 3
                Straight,    // W 3 3
                Straight,    // W 2 3
                Straight,    // W 1 3
                RightCorner, // N 0 3
                Straight,    // N 0 2
                Straight,    // N 0 1
                RightCorner, // E 0 0
            };

            #endregion

            Queue<Track> tracks = new();
            //tracks.Enqueue(new Track("Izolirani", izoliraniSections));
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
