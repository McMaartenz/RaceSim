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

            Assert.AreEqual(queue.Count, sectionTypes.Length);
        }

        [Test]
        public void ConvertToSections_Array_ReturnSameSectionType()
        {
            SectionType[] sectionTypes = { Straight, LeftCorner, RightCorner, StartGrid };

            var sections = Track.ConvertToSections(sectionTypes);

            int i = 0;
            foreach (var section in sections)
            {
                Assert.AreEqual(section.SectionType, sectionTypes[i++]);
            }
        }
    }
}
