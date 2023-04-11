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

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (StackableItem.Count != Count)
                {
                    StackableItem.Count = Count;

                    switch (StackableItem.Parent)
                    {
                        case Tile tile:

                            Context.AddCommand(new TileRefreshItemCommand(tile, StackableItem) );

                            break;

                        case Inventory inventory:

                            Context.AddCommand(new InventoryRefreshItemCommand(inventory, StackableItem) );
                   
                            break;

                        case Container container:

                            Context.AddCommand(new ContainerRefreshItemCommand(container, StackableItem) );

                            break;
                    }
                }

                resolve();
            } );
        }
    }
}