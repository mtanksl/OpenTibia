using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromInventoryToContainerCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromInventoryToContainerCommand(Player player, byte fromSlot, ushort itemId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                Item fromItem = fromInventory.GetContent(FromSlot) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        if (IsMoveable(context, fromItem, Count) )
                        {
                            context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toContainer, ToContainerIndex, Count, true) ).Then(ctx =>
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