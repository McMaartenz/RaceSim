using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    internal class Controller_Data_GenerateTracks_Should
    {
        [Test]
        public void GenerateTracks_Generate_Tracks()
        {
            Data.Competition = new();
            Data.GenerateTracks();

            Assert.That(Data.Competition.Tracks, Is.Not.Null);
        }
    }
}
