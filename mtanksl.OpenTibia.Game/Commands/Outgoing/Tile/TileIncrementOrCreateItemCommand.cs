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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (Tile.TopItem != null)
                {
                    if (Tile.TopItem.Metadata.OpenTibiaId == OpenTibiaId)
                    {
                        if (Tile.TopItem is StackableItem toStackableItem)
                        {
                            if (toStackableItem.Count + Count > 100)
                            {
                                context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, (byte)(toStackableItem.Count + Count - 100) ) );

                                context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );
                            }
                            else
                            {
                                context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + Count) ) );
                            }
                        }
                        else
                        {
                            context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count) );
                        }
                    }
                    else
                    {
                        context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count) );
                    }
                }
                else
                {
                    context.AddCommand(new TileCreateItemCommand(Tile, OpenTibiaId, Count) );                    
                }

                resolve(context);
            } );
        }
    }
}