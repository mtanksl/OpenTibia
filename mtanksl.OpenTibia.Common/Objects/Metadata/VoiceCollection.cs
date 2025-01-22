namespace OpenTibia.Common.Objects
{
    public class VoiceCollection
    {
        public int Interval { get; set; }

        public double Chance { get; set; }

        public VoiceItem[] Items { get; set; }
    }
}