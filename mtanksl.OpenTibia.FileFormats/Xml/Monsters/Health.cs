using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class Health
    {
        [XmlAttribute("now")]
        public int Now { get; set; }

        [XmlAttribute("max")]
        public int Max { get; set; }
    }
}