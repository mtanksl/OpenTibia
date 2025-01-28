using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    // This format does not make much sense. Only need 1 node with multiple attributes.

    public class FlagItem
    {
        [XmlAttribute("summonable")]
        public int? Summonable { get; set; }

        [XmlAttribute("attackable")]
        public int? Attackable { get; set; }

        [XmlAttribute("hostile")]
        public int? Hostile { get; set; }

        [XmlAttribute("illusionable")]
        public int? Illusionable { get; set; }

        [XmlAttribute("convinceable")]
        public int? Convinceable { get; set; }

        [XmlAttribute("pushable")]
        public int? Pushable { get; set; }

        [XmlAttribute("canpushitems")]
        public int? CanPushItems { get; set; }

        [XmlAttribute("canpushcreatures")]
        public int? CanPushCreatures { get; set; }
    }
}