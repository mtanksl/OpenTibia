using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateOutfitEventArgs : GameEventArgs
    {
        public CreatureUpdateOutfitEventArgs(Creature creature, Outfit baseOutfit, Outfit outfit)
        {
            Creature = creature;

            BaseOutfit = baseOutfit;

            Outfit = outfit;
        }

        public Creature Creature { get; }

        public Outfit BaseOutfit { get; }

        public Outfit Outfit { get; }
    }
}