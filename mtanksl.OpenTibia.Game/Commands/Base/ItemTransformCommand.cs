using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : Command
    {
        public ItemTransformCommand(Item fromItem, ushort toOpenTibiaId)
        {
            FromItem = fromItem;

            ToOpenTibiaId = toOpenTibiaId;
        }

        public Item FromItem { get; set; }

        public ushort ToOpenTibiaId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Item toItem = server.ItemFactory.Create(ToOpenTibiaId);

            if (toItem != null)
            {
                Command command = null;

                switch (FromItem.Container)
                {
                    case Tile tile:

                        command = new TileReplaceItemCommand(tile, tile.GetIndex(FromItem), toItem);

                        break;

                    case Inventory inventory:

                        command = new InventoryReplaceItemCommand(inventory, inventory.GetIndex(FromItem), toItem);

                        break;

                    case Container container:

                        command = new ContainerReplaceItemCommand(container, container.GetIndex(FromItem), toItem);

                        break;
                }

                if (command != null)
                {
                    command.Completed += (s, e) =>
                    {
                        //Act

                        base.Execute(e.Server, e.Context);
                    };

                    command.Execute(server, context);
                }
            }
        }
    }
}