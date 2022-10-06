using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using NUnit.Framework;

using static Model.SectionType;

namespace ControllerTest
{
    [TestFixture]
    internal class Model_Competition_ConvertToSections_Should
    {
        [Test]
        public void ConvertToSections_Array_ReturnSameLength()
        {
            SectionType[] sectionTypes = { Straight, LeftCorner };

            var queue = Track.ConvertToSections(sectionTypes);

            Assert.That(sectionTypes, Has.Length.EqualTo(queue.Count));
        }

        [Test]
        public void ConvertToSections_Array_ReturnSameSectionType()
        {
            SectionType[] sectionTypes = { Straight, LeftCorner, RightCorner, StartGrid };

            var sections = Track.ConvertToSections(sectionTypes);

            int i = 0;
            foreach (var section in sections)
            {
                Assert.That(sectionTypes[i++], Is.EqualTo(section.SectionType));
            }
        }
    }
}
