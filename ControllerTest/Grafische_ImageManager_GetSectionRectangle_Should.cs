using ConsoleView;
using Controller;
using Grafische;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Grafische_ImageManager_GetSectionRectangle_Should
    {
        [Test]
        public void GetSectionRectangle_Rectangle_PreservesPoint()
        {
            var point = new System.Drawing.Point(123, 456);

			var rectangle = ImageManager.GetSectionRectangle(point);
            Assert.Multiple(() =>
            {
                Assert.That(rectangle.Location, Is.EqualTo(point));
                Assert.That(rectangle.Width, Is.EqualTo(ImageManager.IMAGE_SIZE));
                Assert.That(rectangle.Height, Is.EqualTo(ImageManager.IMAGE_SIZE));
            });
        }
    }
}
