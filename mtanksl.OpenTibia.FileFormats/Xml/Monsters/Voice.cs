using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class Voice
    {
        [XmlAttribute("sentence")]
        public string Sentence { get; set; }
    }
}