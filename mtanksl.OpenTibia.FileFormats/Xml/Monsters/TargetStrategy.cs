using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class TargetStrategy
    {
        [XmlAttribute("nearest")]
        public double Nearest { get; set; }

        [XmlAttribute("weakest")]
        public double Weakest { get; set; }

        [XmlAttribute("mostdamaged")]
        public double MostDamaged { get; set; }

        [XmlAttribute("random")]
        public double Random { get; set; }
    }
}