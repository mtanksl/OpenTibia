using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Npcs
{
    public class VoiceItem
    {
        [XmlAttribute("sentence")]
        public string Sentence { get; set; }
    }
}