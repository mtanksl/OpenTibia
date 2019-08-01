using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : Command
    {
        public ItemTransformCommand(Item item, ushort openTibiaId)
        {
            Item = item;

            OpenTibiaId = openTibiaId;
        }

        public Item Item { get; set; }

        public ushort OpenTibiaId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Item toItem = server.ItemFactory.Create(OpenTibiaId);

            if (toItem != null)
            {
                //Act

                switch (Item.Container)
                {
                    case Tile tile:

                        new TileReplaceItemCommand(tile, Item, toItem).Execute(server, context);

                        break;

                    case Inventory inventory:

                        new InventoryReplaceItemCommand(inventory, Item, toItem).Execute(server, context);

                        break;

                    case Container container:

                        new ContainerReplaceItemCommand(container, Item, toItem).Execute(server, context);

                        break;
                } 

                //Notify

                base.Execute(server, context);
            }
        }
    }
}