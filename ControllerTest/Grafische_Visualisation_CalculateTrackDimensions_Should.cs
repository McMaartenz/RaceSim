using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    using static Grafische.ImageManager;
    using static SectionType;

    [TestFixture]
    internal class Grafische_ImageManager_CalculateTrackDimensions_Should
    {
        Race race;
        Track track;

        [SetUp]
        public void Setup()
        { 
            track = new("Izolirani", new SectionType[]
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
            });

            race = new(track, new(){ new Skater("A", 0, null, TeamColor.Red), new Skater("B", 0, null, TeamColor.Blue) });
        }

        [Test]
        public void CalculateTrackDimensions_Width()
        {
            (int w, int h) = CalculateTrackDimensions(track);

            Assert.That(w, Is.EqualTo(180));
            Assert.That(h, Is.EqualTo(180));
        }
    }
}
