using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class TileAddItemEventArgs : GameEventArgs
    {
        public TileAddItemEventArgs(Tile tile, Item item, int index)
        {
            Tile = tile;

            Item = item;

            Index = index;
        }

        public Tile Tile { get; }

        public Item Item { get; }

        public int Index { get; }
    }
}