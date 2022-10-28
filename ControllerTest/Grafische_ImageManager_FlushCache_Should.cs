using Grafische;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    internal class Grafische_ImageManager_FlushCache_Should
    {
        [Test]
        public void FlushCache_ImageCache_Empty()
        {
            ImageManager.FlushCache();

            Assert.That(ImageManager.imageCache, Is.Empty);
        }
    }
}
