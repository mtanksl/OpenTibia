using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    // This format does not make much sense. Only need 1 node with multiple attributes.

    public class ElementItem
    {
        [XmlAttribute("physicalPercent")]
        public int? PhysicalPercent { get; set; }

        [XmlAttribute("earthpercent")]
        public int? Earthpercent { get; set; }

        [XmlAttribute("firePercent")]
        public int? FirePercent { get; set; }

        [XmlAttribute("energyPercent")]
        public int? EnergyPercent { get; set; }

        [XmlAttribute("icePercent")]
        public int? IcePercent { get; set; }

        [XmlAttribute("deathPercent")]
        public int? DeathPercent { get; set; }

        [XmlAttribute("holyPercent")]
        public int? HolyPercent { get; set; }

        [XmlAttribute("drownPercent")]
        public int? DrownPercent { get; set; }

        [XmlAttribute("manaDrainPercent")]
        public int? ManaDrainPercent { get; set; }

        [XmlAttribute("lifeDrainPercent")]
        public int? LifeDrainPercent { get; set; }
    }
}