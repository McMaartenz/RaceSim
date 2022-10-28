using Grafische;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Grafische_ImageManager_ToRadians_Should
    {
        [Test]
        public void ToRadians_ReturnZero()
        {
            var radians = ImageManager.ToRadians(0);

            Assert.That(radians, Is.EqualTo(0.0));
        }

        [Test]
        public void ToRadians_ReturnPi()
        {
            var radians = ImageManager.ToRadians(180);

            Assert.That(radians, Is.EqualTo(Math.PI));
        }
    }
}
