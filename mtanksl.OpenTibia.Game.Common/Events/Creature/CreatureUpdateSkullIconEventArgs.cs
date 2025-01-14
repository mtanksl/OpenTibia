using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateSkullIconEventArgs : GameEventArgs
    {
        public CreatureUpdateSkullIconEventArgs(Creature creature, SkullIcon skullIcon)
        {
            Creature = creature;

            SkullIcon = skullIcon;
        }

        public Creature Creature { get; }

        public SkullIcon SkullIcon { get; }
    }
}