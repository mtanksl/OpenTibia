using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    // This format does not make much sense. Only need 1 node with multiple attributes.

    public class ImmunityItem
    {
        [XmlAttribute("physical")]
        public int? Physical { get; set; }

        [XmlAttribute("earth")]
        public int? Earth { get; set; }

        [XmlAttribute("fire")]
        public int? Fire { get; set; }

        [XmlAttribute("energy")]
        public int? Energy { get; set; }

        [XmlAttribute("ice")]
        public int? Ice { get; set; }

        [XmlAttribute("death")]
        public int? Death { get; set; }

        [XmlAttribute("holy")]
        public int? Holy { get; set; }

        [XmlAttribute("drown")]
        public int? Drown { get; set; }

        [XmlAttribute("manaDrain")]
        public int? ManaDrain { get; set; }

        [XmlAttribute("lifeDrain")]
        public int? LifeDrain { get; set; }
    }
}