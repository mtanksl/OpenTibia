using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileCreateItemOrIncrementCommand : Command
    {
        public TileCreateItemOrIncrementCommand(Tile tile, ushort openTibiaId, byte count)
        {
            Tile = tile;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Tile Tile { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            if (Tile.TopItem != null && Tile.TopItem.Metadata.OpenTibiaId == OpenTibiaId && Tile.TopItem is StackableItem stackableItem)
            {
                if (stackableItem.Count + Count > 100)
                { 
                    return Context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, 100) ).Then( () =>
                    {
                        return Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, (byte)(stackableItem.Count + Count - 100) ) );
                    } );
                }

                return Context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count + Count) ) );
            }

            return Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count) );
        }
    }
}