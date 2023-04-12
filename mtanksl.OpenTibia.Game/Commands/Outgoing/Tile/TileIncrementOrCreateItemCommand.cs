using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileIncrementOrCreateItemCommand : Command
    {
        public TileIncrementOrCreateItemCommand(Tile tile, ushort openTibiaId, byte count)
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
            if (Tile.TopItem != null)
            {
                if (Tile.TopItem.Metadata.OpenTibiaId == OpenTibiaId)
                {
                    if (Tile.TopItem is StackableItem toStackableItem)
                    {
                        if (toStackableItem.Count + Count > 100)
                        {
                            Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, (byte)(toStackableItem.Count + Count - 100)));

                            Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100));
                        }
                        else
                        {
                            Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + Count)));
                        }
                    }
                    else
                    {
                        Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count));
                    }
                }
                else
                {
                    Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count));
                }
            }
            else
            {
                Context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count));
            }

            return Promise.Completed;
        }
    }
}