using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class LootItem
    {
        [XmlAttribute("id")]
        public ushort Id { get; set; }

        [XmlAttribute("countmin")]
        public int? CountMin { get; set; }

        [XmlAttribute("countmax")]
        public int? CountMax { get; set; }

        // [XmlAttribute("chance")]
        // public int Chance { get; set; }

        [XmlAttribute("killsToGetOne")]
        public int KillsToGetOne { get; set; }
    }
}