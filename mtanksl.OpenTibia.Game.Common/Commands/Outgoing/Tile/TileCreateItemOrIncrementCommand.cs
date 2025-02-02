using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

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
            if (Tile.TopItem != null && Tile.TopItem is StackableItem stackableItem && stackableItem.Metadata.OpenTibiaId == OpenTibiaId)
            {
                int total = stackableItem.Count + Count;

                if (total > 100)
                { 
                    return Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, (byte)(total - 100) ) ).Then( (item) =>
                    {
                        return Context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, 100) );
                    } );
                }

                return Context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, (byte)total) );
            }

            return Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count) );
        }
    }
}