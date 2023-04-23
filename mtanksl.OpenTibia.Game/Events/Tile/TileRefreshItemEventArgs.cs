using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class TileRefreshItemEventArgs : GameEventArgs
    {
        public TileRefreshItemEventArgs(Tile tile, Item item, byte index)
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