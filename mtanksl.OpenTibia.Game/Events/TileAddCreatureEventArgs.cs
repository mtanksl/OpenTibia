using OpenTibia.Common.Objects;

namespace OpenTibia.Common.Events
{
    public class TileAddCreatureEventArgs : GameEventArgs
    {
        public TileAddCreatureEventArgs(Tile tile, Creature creature, byte index)
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