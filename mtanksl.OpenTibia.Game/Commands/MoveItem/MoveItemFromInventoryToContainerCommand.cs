using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToContainerCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToContainerCommand(Player player, byte fromSlot, byte toContainerId, byte toContainerIndex, byte count)
        {
            Player = player;

            FromSlot = fromSlot;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null)
            {
                Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                if (toContainer != null)
                {
                    //Act

                    RemoveItem(fromInventory, fromItem, server, context);

                    AddItem(toContainer, fromItem, server, context);
                }
            }
        }
    }
}