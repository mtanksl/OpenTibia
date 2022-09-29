using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateCommand : Command
    {
        public ItemUpdateCommand(Item item, byte count)
        {
            Item = item;

            Count = count;
        }

        public Item Item { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            if (Item is StackableItem stackableItem)
            {
                stackableItem.Count = Count;
            }
            else if (Item is FluidItem fluidItem)
            {
                fluidItem.FluidType = (FluidType)Count;
            }

            switch (Item.Parent)
            {
                case Container container:

                    context.AddCommand(new ContainerUpdateItemCommand(container, Item), ctx =>
                    {
                        base.Execute(ctx);
                    } );

                    break;

                case Inventory inventory:

                    context.AddCommand(new InventoryUpdateItemCommand(inventory, Item), ctx =>
                    {
                        base.Execute(ctx);
                    } );
                   
                    break;

                case Tile tile:

                    context.AddCommand(new TileUpdateItemCommand(tile, Item), ctx =>
                    {
                        base.Execute(ctx);
                    } );
                  
                    break;
            }
        }
    }
}