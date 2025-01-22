using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class VoiceItem
    {
        [XmlAttribute("sentence")]
        public string Sentence { get; set; }

        [XmlAttribute("yell")]
        public int? Yell { get; set; }
    }
}