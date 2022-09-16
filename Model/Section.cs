using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum ESectionType
    {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }

    public class SectionData
    {
        public IParticipant Left { get; set; }
        public IParticipant Light { get; set; }
        public int DistanceLeft { get; set; }
        public int DistanceRight { get; set; }
    }

    public class Section
    {
        public ESectionType SectionType { get; set; }

        public Section(ESectionType sectionType)
        {
            SectionType = sectionType;
        }
    }
}
