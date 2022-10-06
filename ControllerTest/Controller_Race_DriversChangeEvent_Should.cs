using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Model.SectionType;

namespace ControllerTest
{
    [TestFixture]
    public class Controller_Race_DriversChangeEvent_Should
    {
        private static Track track;
        private static Race race;

        [SetUp]
        public void Setup()
        {
            track = new("Test track", new SectionType[] { Straight, StartGrid, StartGrid });

            IEquipment fastEquipment = new Skates(100, 100, 100, false);
            IEquipment slowEquipment = new Skates(1, 1, 1, false);
            IEquipment immovableEquipment = new Skates(0, 0, 0, false);

            Skater skaterA = new("A", 0, fastEquipment, default);
            Skater skaterB = new("B", 0, slowEquipment, default);

            race = new(track, new List<IParticipant> { new Skater("C", 0, immovableEquipment, default), skaterA, skaterB });

            race.Positions = new();
            foreach (Section section in track.Sections)
            {
                race.Positions.Add(section, new());
            }

            race.SetStartingPositions();
        }

        [Test]
        public void DriversChangeEvent_FastGoesFast()
        {
            for (int i = 0; i < 15; i++)
            {
                race.OnTimedEvent(this, new());
            }

        }
    }
}
