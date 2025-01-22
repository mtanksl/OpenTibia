using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class DefenseCollection
    {
        [XmlAttribute("armor")]
        public int Armor { get; set; }

        [XmlAttribute("defense")]
        public int Defense { get; set; }

        [XmlArrayItem("defense")]
        public List<DefenseItem> Items { get; set; }
    }
}