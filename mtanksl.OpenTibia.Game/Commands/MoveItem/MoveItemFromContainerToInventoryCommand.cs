using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, byte toSlot, byte count)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ToSlot = toSlot;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public byte ToSlot { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

            if (fromItem != null)
            {
                Inventory toInventory = Player.Inventory;

                Item toItem = toInventory.GetContent(ToSlot) as Item;

                if (toItem == null)
                {
                    //Act

                    RemoveItem(fromContainer, fromItem, server, context);

                    AddItem(toInventory, ToSlot, fromItem, server, context);
                }
            }
        }
    }
}