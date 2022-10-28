using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    using static Grafische.Visualisation;

    [TestFixture]
    internal class Grafische_Visualisation_IsHorizontal_Should
    {
        [Test]
        public void IsHorizontal_West_True()
        {
            bool horizontal = IsHorizontal(Orientation.WEST);

            Assert.That(horizontal, Is.True);
        }

        [Test]
        public void IsHorizontal_East_True()
        {
            bool horizontal = IsHorizontal(Orientation.EAST);

            Assert.That(horizontal, Is.True);
        }

        [Test]
        public void IsHorizontal_North_False()
        {
            bool horizontal = IsHorizontal(Orientation.NORTH);

            Assert.That(horizontal, Is.False);
        }

        [Test]
        public void IsHorizontal_South_False()
        {
            bool horizontal = IsHorizontal(Orientation.SOUTH);

            Assert.That(horizontal, Is.False);
        }
    }
}
