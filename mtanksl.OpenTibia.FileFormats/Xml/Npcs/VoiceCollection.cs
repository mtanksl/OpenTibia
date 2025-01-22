using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Npcs
{
    public class VoiceCollection
    {
        [XmlAttribute("interval")]
        public int Interval { get; set; }

        [XmlAttribute("chance")]
        public double Chance { get; set; }

        [XmlArrayItem("voice")]
        public List<VoiceItem> Items { get; set; }
    }
}