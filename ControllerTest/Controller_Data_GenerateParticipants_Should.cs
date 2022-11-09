using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Controller;

namespace ControllerTest
{
    [TestFixture]
    internal class Controller_Data_GenerateParticipants_Should
    {
        [Test]
        public void GenerateParticipants_Return_Participants()
        {
            Data.Competition = new();
            Data.GenerateParticipants();

            Assert.Multiple(() =>
            {
                Assert.That(Data.Competition.Participants, Is.Not.Empty);
                Assert.That(Data.Competition.Participants[0].Equipment, Is.Not.Null);
            });
        }
    }
}
