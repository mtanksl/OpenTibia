using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromContainerToInventoryCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort fromItemId, byte toSlot, ushort toItemId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            FromItemId = fromItemId;

            ToSlot = toSlot;

            ToItemId = toItemId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort FromItemId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Inventory toInventory = Player.Inventory;

                    Item toItem = toInventory.GetContent(ToSlot) as Item;

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