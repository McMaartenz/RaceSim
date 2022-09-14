using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleView
{
    public class Track
    {
        public Competition? competition;
        public Section[] sections;
    }
    public class Section
    {
        public Track track;
    }
}
