using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class TileRemoveCreatureEventArgs : GameEventArgs
    {
        public TileRemoveCreatureEventArgs(Tile tile, Creature creature, byte index)
        {
            Tile = tile;

            Creature = creature;

            Index = index;
        }

        public Tile Tile { get; set; }

        public Creature Creature { get; set; }

        public byte Index { get; set; }
    }
}