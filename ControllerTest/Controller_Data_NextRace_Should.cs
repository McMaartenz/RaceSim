using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    internal class Controller_Data_NextRace_Should
    {
        private volatile bool eventCalled;

        [SetUp]
        public void Setup()
        {
            eventCalled = false;

            Data.Initialize();
            Data.RaceChanged += Data_RaceChanged;
        }

        private void Data_RaceChanged(object? sender, RaceChangedEventArgs e)
        {
            eventCalled = true;
        }

        [Test]
        public void NextRace_()
        {
            Race prev = Data.CurrentRace;
            Data.NextRace();
            Race next = Data.CurrentRace;

            Assert.Multiple(() =>
            {
                Assert.That(next, Is.Not.Null);
                Assert.That(prev, Is.Not.EqualTo(next));
                Assert.That(eventCalled, Is.True);
            });
        }
    }
}
