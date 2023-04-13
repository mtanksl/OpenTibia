using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromContainerToInventoryCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte toSlot, byte count) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;
            
            ToSlot = toSlot;

            Count = count;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToSlot { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Inventory toInventory = Player.Inventory;

                    if (IsMoveable(fromItem, Count) )
                    {
                        return Context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toInventory, ToSlot, Count, true) );
                    }
                }
            }

            return Promise.Break;
        }
    }
}