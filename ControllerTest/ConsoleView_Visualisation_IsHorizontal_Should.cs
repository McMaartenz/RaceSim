using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using ConsoleView;

namespace ControllerTest
{
    [TestFixture]
    internal class ConsoleView_Visualisation_IsHorizontal_Should
    {
        [Test]
        public void IsHorizontal_OrientationSouth_ReturnFalse()
        {
            Assert.That(Visualisation.IsHorizontal(Orientation.SOUTH), Is.False);
        }

        [Test]
        public void IsHorizontal_OrientationWest_ReturnTrue()
        {
            Assert.That(Visualisation.IsHorizontal(Orientation.WEST), Is.True);
        }
    }
}
