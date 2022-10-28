using Grafische;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    internal class Grafische_ImageManager_CreateEmptyImage_Should
    {
        [SetUp]
        public void Setup()
        {
            ImageManager.FlushCache();
        }

        [Test]
        public void CreateEmptyImage_ImageCache_Contains()
        {
            ImageManager.CreateEmptyImage(123, 456);

            Assert.That(ImageManager.imageCache.ContainsKey("empty"));
        }

        [Test]
        public void CreateEmptyImage_Bitmap_CorrectSize()
        {
            ImageManager.CreateEmptyImage(123, 456);

            var bitmap = ImageManager.imageCache["empty"];
            Assert.Multiple(() =>
            {
                Assert.That(bitmap.Width, Is.EqualTo(123));
                Assert.That(bitmap.Height, Is.EqualTo(456));
            });
        }
    }
}
