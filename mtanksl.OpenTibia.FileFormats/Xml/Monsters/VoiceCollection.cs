using System.Collections.Generic;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class VoiceCollection
    {
        public int Interval { get; set; }

        public double Chance { get; set; }

        public List<VoiceItem> Items { get; set; }
    }
}