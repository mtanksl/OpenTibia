using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class DefenseItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("interval")]
        public int Interval { get; set; }

        [XmlAttribute("chance")]
        public double Chance { get; set; }

        [XmlAttribute("min")]
        public int? Min { get; set; }

        [XmlAttribute("max")]
        public int? Max { get; set; }
    }
}