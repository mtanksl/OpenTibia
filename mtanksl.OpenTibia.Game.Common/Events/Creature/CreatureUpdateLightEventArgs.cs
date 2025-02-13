using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateLightEventArgs : GameEventArgs
    {
        public CreatureUpdateLightEventArgs(Creature creature, Light conditionLight)
        {
            Creature = creature;

            ConditionLight = conditionLight;
        }

        public Creature Creature { get; }

        public Light ConditionLight { get; }
    }
}