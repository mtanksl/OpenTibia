using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdatePartyIconEventArgs : GameEventArgs
    {
        public CreatureUpdatePartyIconEventArgs(Creature creature, PartyIcon partyIcon)
        {
            Creature = creature;

            PartyIcon = partyIcon;
        }

        public Creature Creature { get; set; }

        public PartyIcon PartyIcon { get; set; }
    }
}