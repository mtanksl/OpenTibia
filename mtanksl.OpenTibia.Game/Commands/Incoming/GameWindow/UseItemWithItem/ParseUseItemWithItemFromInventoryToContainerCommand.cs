using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromInventoryToContainerCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromInventoryToContainerCommand(Player player, byte fromSlot, ushort fromItemId, byte toContainerId, byte toContainerIndex, ushort toItemId) :base(player)
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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
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
                            if ( IsUseable(context, fromItem) )
                            {
                                context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) ).Then(ctx =>
                                {
                                    resolve(context);
                                } );
                            }
                        }
                    }
                }
            } );
        }
    }
}