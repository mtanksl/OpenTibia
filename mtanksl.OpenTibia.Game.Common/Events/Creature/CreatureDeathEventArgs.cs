using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureDeathEventArgs : GameEventArgs
    {
        public CreatureDeathEventArgs(Tile tile, Creature creature)
        {
            Tile = tile;

            Creature = creature;
        }

        public Tile Tile { get; }

        public Creature Creature { get; }
    }
}