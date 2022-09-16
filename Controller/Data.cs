﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Model;

using static Model.ESectionType;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }
        
        public static void GenerateParticipants()
        {
            List<IParticipant> participants = new();
            participants.Add(new Skater("Jan", 0, new Skates(100, 100, 100, false), ETeamColor.Red));
            participants.Add(new Skater("Sam", 0, new Skates(200, 50, 100, false), ETeamColor.Blue));
            participants.Add(new Skater("Sem", 0, new Skates(50, 125, 200, false), ETeamColor.Grey));

            Competition.Participants = participants;
        }

        public static void GenerateTracks()
        {
            ESectionType[] zwolleSections =
            {
                StartGrid, // 0 0
                Finish, // 1 0
                Straight, // 2 0
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

            ESectionType[] enschedeSections =
            {
                StartGrid, // 0 0
                Finish, // 1 0
                Straight, // 2 0
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
            Track nextTrack = Competition.NextTrack();
            if (nextTrack is not null)
            {
                CurrentRace = new(Competition.NextTrack(), Competition.Participants);
            }
        }
    }
}
