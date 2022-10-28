using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    using static SectionType;
    using static Grafische.Visualisation;

    [TestFixture]
    internal class Grafische_Visualisation_TrackIterator_Iterator_Should
    {
        Track track;
        Race race;

        [SetUp]
        public void Setup()
        {
            track = new Track("ABC", new SectionType[] { StartGrid, StartGrid, Finish, RightCorner, RightCorner, Straight, Straight, Straight, RightCorner, RightCorner});
            race = new(track, new());
        }

        [Test]
        public void Iterator_Orientations_Correct()
        {
            Queue<Orientation> capturedOrientations = new();

            TrackIterator iter = new(track, race);
            foreach (TrackIterator.TrackIteration el in iter.Iterator())
            {
                capturedOrientations.Enqueue(el.dir);
            }

            Queue<Orientation> expectedOrientations = new(new List<Orientation>{
                Orientation.EAST,
                Orientation.EAST,
                Orientation.EAST,
                Orientation.SOUTH,
                Orientation.WEST,
                Orientation.WEST,
                Orientation.WEST,
                Orientation.WEST,
                Orientation.NORTH,
                Orientation.EAST
            });

            Assert.That(capturedOrientations, Is.EquivalentTo(expectedOrientations));
        }
    }
}
