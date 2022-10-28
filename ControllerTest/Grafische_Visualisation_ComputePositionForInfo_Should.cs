using ConsoleView;
using Grafische;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    using static Grafische.Visualisation;

    [TestFixture]
    internal class Grafische_Visualisation_ComputePositionForInfo_Should
    {
        PlayerRenderer infoHorizontal;
        PlayerRenderer infoVertical;
        PlayerRenderer infoHorizontalMirror;
        PlayerRenderer infoVerticalMirror;
        PlayerRenderer infoRightTurn;
        PlayerRenderer infoLeftTurn;

        [SetUp]
        public void Setup()
        {
            Point point = Point.Empty;

            IParticipant left = new Skater("A", 0, new Skates(0, 0, 0, false), TeamColor.Red);
            IParticipant right = new Skater("B", 0, new Skates(0, 0, 0, false), TeamColor.Blue);

            Section straight = new(SectionType.Straight);
            Section turnRight = new(SectionType.RightCorner);
            Section turnLeft = new(SectionType.LeftCorner);
            SectionData data = new() { Left = left, Right = right, DistanceLeft = 50, DistanceRight = 50 };

            infoHorizontal = new(point, straight, data, Orientation.EAST, false);
            infoVertical = new(point, straight, data, Orientation.SOUTH, false);
            infoHorizontalMirror = new(point, straight, data, Orientation.WEST, true);
            infoVerticalMirror = new(point, straight, data, Orientation.NORTH, true);

            infoRightTurn = new(point, turnRight, data, Orientation.EAST, false);
            infoLeftTurn = new(point, turnLeft, data, Orientation.EAST, false);
        }

        [Test]
        public void ComputePositionForInfo_Horizontal_XY()
        {
            (int x, int y) = ComputePositionForInfo(infoHorizontal, true);
            
            Assert.Multiple(() =>
            {
                Assert.That(x, Is.EqualTo(40));
                Assert.That(y, Is.EqualTo(0));
            });
        }

        [Test]
        public void ComputePositionForInfo_Vertical_XY()
        {
            (int x, int y) = ComputePositionForInfo(infoVertical, true);

            Assert.Multiple(() =>
            {
                Assert.That(x, Is.EqualTo(64));
                Assert.That(y, Is.EqualTo(40));
            });
        }

        [Test]
        public void ComputePositionForInfo_HorizontalMirror_XY()
        {
            (int x, int y) = ComputePositionForInfo(infoHorizontalMirror, true);

            Assert.Multiple(() =>
            {
                Assert.That(x, Is.EqualTo(88));
                Assert.That(y, Is.EqualTo(64));
            });
        }

        [Test]
        public void ComputePositionForInfo_VerticalMirror_XY()
        {
            (int x, int y) = ComputePositionForInfo(infoVerticalMirror, true);

            Assert.Multiple(() =>
            {
                Assert.That(x, Is.EqualTo(0));
                Assert.That(y, Is.EqualTo(88));
            });
        }

        [Test]
        public void ComputePositionForInfo_RightTurn_XY()
        {
            (int x, int y) = ComputePositionForInfo(infoRightTurn, true);

            Assert.Multiple(() =>
            {
                Assert.That(x, Is.EqualTo(16));
                Assert.That(y, Is.EqualTo(57));
            });
        }

        [Test]
        public void ComputePositionForInfo_LeftTurn_XY()
        {
            (int x, int y) = ComputePositionForInfo(infoLeftTurn, true);

            Assert.Multiple(() =>
            {
                Assert.That(x, Is.EqualTo(87));
                Assert.That(y, Is.EqualTo(-15));
            });
        }
    }
}
