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
        private static Skater skaterA;
        private static Skater skaterB;

        [SetUp]
        public void Setup()
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

            track = new("Test", zwolleSections);

            IEquipment fastEquipment = new Skates(100, 100, 100, false);
            IEquipment slowEquipment = new Skates(1, 1, 1, false);

            skaterA = new("A", 0, fastEquipment, default);
            skaterB = new("B", 0, slowEquipment, default);
            IEquipment immovableEquipment = new Skates(0, 0, 0, false);

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

            Assert.That(race.GetSectionData(track.Sections.First.Next.Next.Value).Left, Is.EqualTo(skaterA));
        }
    }
}
