using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateOutfitEventArgs : GameEventArgs
    {
        public CreatureUpdateOutfitEventArgs(Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool conditionStealth)
        {
            Creature = creature;

            BaseOutfit = baseOutfit;

            ConditionOutfit = conditionOutfit;

            Swimming = swimming;

            ConditionStealth = conditionStealth;
        }

        public Creature Creature { get; }

        public Outfit BaseOutfit { get; set; }

        public Outfit ConditionOutfit { get; set; }

        public bool Swimming { get; set; }

        public bool ConditionStealth { get; set; }
    }
}