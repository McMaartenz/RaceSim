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
    public class Grafische_ImageManager_GetSkaterRectangle_Should
    {
        [Test]
        public void GetSkaterRectangle_Rectangle_PreservesXY()
        {
            var point = new System.Drawing.Point(123, 456);

            var rectangle = ImageManager.GetSkaterRectangle(point.X, point.Y);
            Assert.Multiple(() =>
            {
                Assert.That(rectangle.Location, Is.EqualTo(point));
                Assert.That(rectangle.Width, Is.EqualTo(ImageManager.SKATER_SIZE));
                Assert.That(rectangle.Height, Is.EqualTo(ImageManager.SKATER_SIZE));
            });
        }
    }
}
