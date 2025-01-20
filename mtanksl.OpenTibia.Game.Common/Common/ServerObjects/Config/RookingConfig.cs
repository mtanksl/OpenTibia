using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class RookingConfig
    {
        public bool Enabled { get; set; }

        public ulong ExperienceThreshold { get; set; }

        public Position PlayerNewPosition { get; set; }
    }
}