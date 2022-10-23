using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class StackableItemUpdateCountCommand : Command
    {
        public StackableItemUpdateCountCommand(StackableItem stackableItem, byte count)
        {
            StackableItem = stackableItem;

            Count = count;
        }

        public StackableItem StackableItem { get; set; }

        public byte Count { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (StackableItem.Count != Count)
                {
                    StackableItem.Count = Count;

                    switch (StackableItem.Parent)
                    {
                        case Tile tile:

                            context.AddCommand(new TileRefreshItemCommand(tile, StackableItem) );

                            break;

                        case Inventory inventory:

                            context.AddCommand(new InventoryRefreshItemCommand(inventory, StackableItem) );
                   
                            break;

                        case Container container:

                            context.AddCommand(new ContainerRefreshItemCommand(container, StackableItem) );

                            break;
                    }
                }

                resolve(context);
            } );
        }
    }
}