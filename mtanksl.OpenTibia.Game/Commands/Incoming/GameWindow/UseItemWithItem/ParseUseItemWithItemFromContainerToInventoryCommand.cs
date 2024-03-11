using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromContainerToInventoryCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort fromTibiaId, byte toSlot, ushort toTibiaId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            FromTibiaId = fromTibiaId;

            ToSlot = toSlot;

            ToTibiaId = toTibiaId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort FromTibiaId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.Containers.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromTibiaId)
                {
                    Inventory toInventory = Player.Inventory;

                    Item toItem = toInventory.GetContent(ToSlot) as Item;

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