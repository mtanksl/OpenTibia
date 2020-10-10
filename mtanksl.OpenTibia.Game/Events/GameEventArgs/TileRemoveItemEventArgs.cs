using OpenTibia.Common.Objects;

namespace OpenTibia.Common.Events
{
    public class TileRemoveItemEventArgs : GameEventArgs
    {
        public TileRemoveItemEventArgs(Tile tile, Item item,  byte index)
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