using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class ChangeTargetStrategy
    {
        [XmlAttribute("interval")]
        public int Interval { get; set; }

        [XmlAttribute("chance")]
        public int Chance { get; set; }
    }
}