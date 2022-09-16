using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum SectionType
    {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }

    [DebuggerDisplay("L{Left}R{Right}, P{DistanceLeft}.{DistanceRight}")]
    public class SectionData
    {
        public IParticipant Left { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceLeft { get; set; }
        public int DistanceRight { get; set; }
    }

    [DebuggerDisplay("{SectionType}")]
    public class Section
    {
        public SectionType SectionType { get; set; }

        public Section(SectionType sectionType)
        {
            SectionType = sectionType;
        }
    }
}
