using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class StackableItemUpdateCountCommand : Command
    {
        public StackableItemUpdateCountCommand(StackableItem item, byte count)
        {
            Item = item;

            Count = count;
        }

        public StackableItem Item { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            if (Item.Count != Count)
            {
                Item.Count = Count;

                switch (Item.Parent)
                {
                    case Tile tile:

                        context.AddCommand(new TileRefreshItemCommand(tile, Item) ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );
                  
                        break;

                    case Inventory inventory:

                        context.AddCommand(new InventoryRefreshItemCommand(inventory, Item) ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );
                   
                        break;

                    case Container container:

                        context.AddCommand(new ContainerRefreshItemCommand(container, Item) ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );

                        break;
                }
            }
            else
            {
                OnComplete(context);
            }
        }
    }
}