using OpenTibia.Common.Objects;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ItemMoveCommand : Command
    {
        public ItemMoveCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count)
        {
            Player = player;

            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            if ( !server.Scripts.ItemMoveScripts.Any(script => script.OnItemMove(Player, Item, ToContainer, ToIndex, Count, server, context) ) )
            {
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

                switch (ToContainer)
                {
                    case Tile tile:

                        new TileAddItemCommand(tile, Item).Execute(server, context);

                        break;

                    case Inventory inventory:

                        new InventoryAddItemCommand(inventory, ToIndex, Item).Execute(server, context);

                        break;

                    case Container container:

                        new ContainerAddItemCommand(container, Item).Execute(server, context);

                        break;
                }
            }

            //Notify

            base.Execute(server, context);
        }
    }
}