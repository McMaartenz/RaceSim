using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    internal class Model_Competition_NextTrack_Should
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track? result = _competition.NextTrack();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track newTrack = new("", Array.Empty<SectionType>());
            _competition.Tracks.Enqueue(newTrack);

            Track? result = _competition.NextTrack();
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new("", Array.Empty<SectionType>());

            _competition.Tracks.Enqueue(track);

            Track? resultA = _competition.NextTrack();
            Track? resultB = _competition.NextTrack();
            Assert.Multiple(() =>
            {
                Assert.That(resultA, Is.Not.Null);
                Assert.That(resultB, Is.Null);
            });
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track trackA = new("A", Array.Empty<SectionType>());
            Track trackB = new("B", Array.Empty<SectionType>());

            _competition.Tracks.Enqueue(trackA);
            _competition.Tracks.Enqueue(trackB);

            Track? resultA = _competition.NextTrack();
            Track? resultB = _competition.NextTrack();

            Assert.Multiple(() =>
            {
                Assert.That(resultA, Is.Not.Null);
                Assert.That(resultB, Is.Not.Null);

                Assert.That(resultA.Name, Is.EqualTo("A"));
                Assert.That(resultB.Name, Is.EqualTo("B"));
            });
        }
    }
}
