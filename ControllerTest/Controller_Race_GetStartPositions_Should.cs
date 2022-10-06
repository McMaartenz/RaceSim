using ConsoleView;
using Controller;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
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
    internal class Controller_Race_GetStartPositions_Should
    {
        private static Track track;
        private static Race race;

        [SetUp]
        public void SetUp()
        {
            track = new("Test track", new SectionType[] { Straight, StartGrid, StartGrid});
            race = new(track, new List<IParticipant> {new Skater("A",0,null,default), new Skater("B", 0, null, default), new Skater("C", 0, null, default) });
            
            race.Positions = new();
            foreach (Section section in track.Sections)
            {
                race.Positions.Add(section, new());
            }

            race.SetStartingPositions();
        }

        [Test]
        public void GetStartPositions_ReturnSections()
        {
            Section[] startingPositions = race.GetStartPositions();
            Assert.Multiple(() =>
            {
                Assert.That(track.Sections.First.Next.Value,      Is.EqualTo(startingPositions[0]));
                Assert.That(track.Sections.First.Next.Next.Value, Is.EqualTo(startingPositions[1]));
                Assert.That(startingPositions, Has.Length.EqualTo(2));
            });
        }
    }
}
