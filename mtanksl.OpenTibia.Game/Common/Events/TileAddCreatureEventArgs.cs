using OpenTibia.Common.Objects;
using OpenTibia.Game;

namespace OpenTibia.Common.Events
{
    public class TileAddCreatureEventArgs : GameEventArgs
    {
        public TileAddCreatureEventArgs(Creature creature, Tile tile, byte index, Server server, Context context) : base(server, context)
        {
            Creature = creature;

            Tile = tile;

            Index = index;
        }

        public Creature Creature { get; set; }

        public Tile Tile { get; set; }

        public byte Index { get; set; }
    }
}