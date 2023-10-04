using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class TileRemoveItemEventArgs : GameEventArgs
    {
        public TileRemoveItemEventArgs(Tile tile, Item item, int index)
        {
            Tile = tile;

            Item = item;

            Index = index;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public int Index { get; set; }
    }
}