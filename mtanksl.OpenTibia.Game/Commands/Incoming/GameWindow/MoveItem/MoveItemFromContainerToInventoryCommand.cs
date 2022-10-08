using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte toSlot, byte count) : base(player)
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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

                if (fromContainer != null)
                {
                    Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                    {
                        Inventory toInventory = Player.Inventory;

                        if (IsMoveable(context, fromItem, Count) && IsPickupable(context, fromItem) )
                        {
                            context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toInventory, ToSlot, Count) ).Then(ctx =>
                            {
                                resolve(ctx);
                            } );
                        }
                    }
                }
            } );
        }
    }
}