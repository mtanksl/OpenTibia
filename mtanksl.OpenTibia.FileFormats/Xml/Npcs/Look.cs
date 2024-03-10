using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Npcs
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

        [XmlAttribute("feet")]
        public int Feet { get; set; }
    }
}