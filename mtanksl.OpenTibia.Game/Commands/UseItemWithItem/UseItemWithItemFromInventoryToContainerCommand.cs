using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromInventoryToContainerCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromInventoryToContainerCommand(Player player, byte fromSlot, ushort fromItemId, byte toContainerId, byte toContainerIndex, ushort toItemId) :base(player)
        {
            FromSlot = fromSlot;

            FromItemId = fromItemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToItemId = toItemId;
        }

        public byte FromSlot { get; set; }

        public ushort FromItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
            {
                Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                if (toContainer != null)
                {
                    Item toItem = toContainer.GetContent(ToContainerIndex) as Item;

                    if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                    {
                        //Act

                        if ( IsUseable(fromItem, server, context) )
                        {
                            UseItemWithItem(fromItem, toItem, server, context);
                        }
                    }
                }
            }
        }
    }
}