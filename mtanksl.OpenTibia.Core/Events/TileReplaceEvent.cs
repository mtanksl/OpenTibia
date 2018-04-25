namespace OpenTibia
{
    public class TileReplaceEvent : GameEvent
    {
        public TileReplaceEvent(Tile tile, byte index, Item before, Item after)
        {
            Tile = tile;

            Index = index;

            Before = before;

            After = after;
        }

        public Tile Tile { get; set; }

        public byte Index { get; set; }

        public Item Before { get; set; }

        public Item After { get; set; }
    }
}