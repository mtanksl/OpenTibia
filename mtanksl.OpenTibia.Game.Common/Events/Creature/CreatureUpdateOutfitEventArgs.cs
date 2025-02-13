using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateOutfitEventArgs : GameEventArgs
    {
        public CreatureUpdateOutfitEventArgs(Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool stealth)
        {
            Creature = creature;

            BaseOutfit = baseOutfit;

            ConditionOutfit = conditionOutfit;

            Swimming = swimming;

            Stealth = stealth;
        }

        public Creature Creature { get; }

        public Outfit BaseOutfit { get; set; }

        public Outfit ConditionOutfit { get; set; }

        public bool Swimming { get; set; }

        public bool Stealth { get; set; }
    }
}