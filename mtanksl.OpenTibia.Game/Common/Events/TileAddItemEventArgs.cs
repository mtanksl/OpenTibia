using OpenTibia.Common.Objects;
using OpenTibia.Game;

namespace OpenTibia.Common.Events
{
    public class TileAddItemEventArgs : GameEventArgs
    {
        public TileAddItemEventArgs(Item item, Tile tile, byte index, Server server, Context context) : base(server, context)
        {
            Item = item;

            Tile = tile;

            Index = index;
        }

        public Item Item { get; set; }

        public Tile Tile { get; set; }

        public byte Index { get; set; }
    }
}