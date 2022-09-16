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
            Assert.IsFalse(Visualisation.IsHorizontal(Orientation.SOUTH));
        }

        [Test]
        public void IsHorizontal_OrientationWest_ReturnTrue()
        {
            Assert.IsTrue(Visualisation.IsHorizontal(Orientation.WEST));
        }
    }
}
