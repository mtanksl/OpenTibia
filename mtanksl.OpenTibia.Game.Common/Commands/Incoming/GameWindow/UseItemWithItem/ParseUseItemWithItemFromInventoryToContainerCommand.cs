using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromInventoryToContainerCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromInventoryToContainerCommand(Player player, byte fromSlot, ushort fromTibiaId, byte toContainerId, byte toContainerIndex, ushort toTibiaId) :base(player)
        {
            FromSlot = fromSlot;

            FromTibiaId = fromTibiaId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToTibiaId = toTibiaId;
        }

        public byte FromSlot { get; set; }

        public ushort FromTibiaId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == FromTibiaId)
            {
                Container toContainer = Player.Client.Containers.GetContainer(ToContainerId);

                if (toContainer != null)
                {
                    Item toItem = toContainer.GetContent(ToContainerIndex) as Item;

                    if (toItem != null && toItem.Metadata.TibiaId == ToTibiaId)
                    {
                        if ( IsUseable(fromItem) )
                        {
                            return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}