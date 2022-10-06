using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        public int GetSpeed()
        {
            return (int)((Quality/100.0) * (Performance/100.0) * (Speed/100.0) * 20.0);
        }
    }

    public class Skates : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        public Skates(int quality, int performance, int speed, bool isBroken)
        {
            Quality = quality;
            Performance = performance;
            Speed = speed;
            IsBroken = isBroken;
        }
    }
}
