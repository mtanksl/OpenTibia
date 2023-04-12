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
            if (StackableItem.Count != Count)
            {
                StackableItem.Count = Count;

                switch (StackableItem.Parent)
                {
                    case Tile tile:

                        return Context.AddCommand(new TileRefreshItemCommand(tile, StackableItem) );

                    case Inventory inventory:

                        return Context.AddCommand(new InventoryRefreshItemCommand(inventory, StackableItem) );

                    case Container container:

                        return Context.AddCommand(new ContainerRefreshItemCommand(container, StackableItem) );
                }
            }

            return Promise.Completed;
        }
    }
}