using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromInventoryToContainerCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromInventoryToContainerCommand(Player player, byte fromSlot, ushort tibiaId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
        {
            FromSlot = fromSlot;

            TibiaId = tibiaId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort TibiaId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
            {
                Container toContainer = Player.Client.Containers.GetContainer(ToContainerId);

                if (toContainer != null)
                {
                    if (IsPossible(fromItem, toContainer, ToContainerIndex) && IsPickupable(fromItem) && IsMoveable(fromItem, Count) )
                    {
                        return Context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toContainer, ToContainerIndex, Count, true) );
                    }     
                }
            }

            return Promise.Break;
        }
    }
}