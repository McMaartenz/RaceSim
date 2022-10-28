using Grafische;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    internal class Grafische_ImageManager_CreateImage_Should
    {
        [Test]
        public void CreateImage_Size_Correct()
        {
            var bitmap = ImageManager.CreateImage(123, 456);
            Assert.Multiple(() =>
            {
                Assert.That(bitmap.Width, Is.EqualTo(123));
                Assert.That(bitmap.Height, Is.EqualTo(456));
            });
        }
    }
}
