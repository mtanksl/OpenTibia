using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemDestroyCommand : Command
    {
        public ItemDestroyCommand(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            switch (Item.Container)
            {
                case Tile tile:

                    new TileRemoveItemCommand(tile, Item).Execute(server, context);

                    break;

                case Inventory inventory:

                    new InventoryRemoveItemCommand(inventory, Item).Execute(server, context);

                    break;

                case Container container:

                    new ContainerRemoveItemCommand(container, Item).Execute(server, context);

                    break;
            }

            //Notify

            base.Execute(server, context);            
        }
    }
}
