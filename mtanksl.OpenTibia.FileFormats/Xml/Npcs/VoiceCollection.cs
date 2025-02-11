using System.Collections.Generic;

namespace OpenTibia.FileFormats.Xml.Npcs
{
    public class VoiceCollection
    {
        public int Interval { get; set; }

        public double Chance { get; set; }

        public List<VoiceItem> Items { get; set; }
    }
}