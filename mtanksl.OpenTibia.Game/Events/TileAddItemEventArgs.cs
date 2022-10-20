using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class TileAddItemEventArgs : GameEventArgs
    {
        public TileAddItemEventArgs(Tile tile, Item item, byte index)
        {
            Tile = tile;

            Item = item;

            Index = index;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public byte Index { get; set; }
    }
}