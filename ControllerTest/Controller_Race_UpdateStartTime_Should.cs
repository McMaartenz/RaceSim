using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    internal class Controller_Race_UpdateStartTime_Should
    {
        [SetUp]
        public void Setup()
        {
            Data.Initialize();
        }

        [Test]
        public void UpdateStartTime_Update_Time()
        {
            DateTime start = Data.CurrentRace.StartTime;
            Data.CurrentRace.UpdateStartTime();
            DateTime end = Data.CurrentRace.StartTime;

            Assert.That(end, Is.GreaterThan(start));
        }
    }
}
