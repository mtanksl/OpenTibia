using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateLightEventArgs : GameEventArgs
    {
        public CreatureUpdateLightEventArgs(Creature creature, Light light)
        {
            Creature = creature;

            Light = light;
        }

        public Creature Creature { get; }

        public Light Light { get; }
    }
}