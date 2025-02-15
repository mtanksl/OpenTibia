using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateLightEventArgs : GameEventArgs
    {
        public CreatureUpdateLightEventArgs(Creature creature, Light conditionLight, Light itemLight)
        {
            Creature = creature;

            ConditionLight = conditionLight;

            ItemLight = itemLight;
        }

        public Creature Creature { get; }

        public Light ConditionLight { get; }

        public Light ItemLight { get; set; }
    }
}