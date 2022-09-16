using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum TeamColor
    {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }

    public interface IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColor TeamColor { get; set; }
    }

    public class Skater : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColor TeamColor { get; set; }

        public Skater(string name, int points, IEquipment equipment, TeamColor teamColor)
        {
            Name = name;
            Points = points;
            Equipment = equipment;
            TeamColor = teamColor;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
