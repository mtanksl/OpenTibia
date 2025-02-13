using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class NpcMetadata
    {
        public string Name { get; set; }

        public string NameDisplayed { get; set; }

        public string Description { get; set; }

        public ushort Speed { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public Light Light { get; set; }

        public Outfit Outfit { get; set; }

        public VoiceCollection Voices { get; set; }
    }
}