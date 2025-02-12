using System.Collections.Generic;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class DefenseItem
    {
        public string Name { get; set; }

        public int Interval { get; set; }

        public double Chance { get; set; }

        public int? Min { get; set; }

        public int? Max { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
    }
}