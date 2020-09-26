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

        public override void Execute(Context context)
        {
            //Arrange

            Item toItem = context.Server.ItemFactory.Create(OpenTibiaId);

            if (toItem != null)
            {
                //Act

                switch (Item.Container)
                {
                    case Tile tile:

                        new TileReplaceItemCommand(tile, Item, toItem).Execute(context);

                        break;

                    case Inventory inventory:

                        new InventoryReplaceItemCommand(inventory, Item, toItem).Execute(context);

                        break;

                    case Container container:

                        new ContainerReplaceItemCommand(container, Item, toItem).Execute(context);

                        break;
                } 

                //Notify

                base.Execute(context);
            }
        }
    }
}