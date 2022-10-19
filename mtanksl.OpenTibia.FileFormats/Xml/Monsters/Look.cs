using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class Look
    {
        [XmlAttribute("typeex")]
        public int TypeEx { get; set; }

        [XmlAttribute("type")]
        public int Type { get; set; }

        [XmlAttribute("head")]
        public int Head { get; set; }

        [XmlAttribute("body")]
        public int Body { get; set; }

        [XmlAttribute("legs")]
        public int Legs { get; set; }

        [XmlAttribute("feer")]
        public int Feet { get; set; }

        [XmlAttribute("corpse")]
        public int Corpse { get; set; }
    }
}