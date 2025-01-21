using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class ElementItem
    {
        [XmlAttribute("holyPercent")]
        public int? HolyPercent { get; set; }

        [XmlAttribute("icePercent")]
        public int? IcePercent { get; set; }

        [XmlAttribute("deathPercent")]
        public int? DeathPercent { get; set; }

        [XmlAttribute("physicalPercent")]
        public int? PhysicalPercent { get; set; }

        [XmlAttribute("earthpercent")]
        public int? Earthpercent { get; set; }

        [XmlAttribute("energyPercent")]
        public int? EnergyPercent { get; set; }

        [XmlAttribute("firePercent")]
        public int? FirePercent { get; set; }
    }
}