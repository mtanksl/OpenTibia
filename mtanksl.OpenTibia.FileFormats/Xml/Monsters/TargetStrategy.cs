using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class TargetStrategy
    {
        [XmlAttribute("nearest")]
        public int Nearest { get; set; }

        [XmlAttribute("weakest")]
        public int Weakest { get; set; }

        [XmlAttribute("mostdamaged")]
        public int MostDamaged { get; set; }

        [XmlAttribute("random")]
        public int Random { get; set; }
    }
}