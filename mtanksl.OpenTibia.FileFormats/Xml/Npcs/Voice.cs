using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Npcs
{
    public class Voice
    {
        [XmlAttribute("sentence")]
        public string Sentence { get; set; }
    }
}