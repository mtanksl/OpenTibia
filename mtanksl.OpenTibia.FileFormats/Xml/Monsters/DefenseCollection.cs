using System.Collections.Generic;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class DefenseCollection
    {
        public double Mitigation { get; set; }

        public int Defense { get; set; }

        public int Armor { get; set; }

        public List<DefenseItem> Items { get; set; }
    }
}