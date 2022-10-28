using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromContainerToContainerCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromContainerToContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

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
                        Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                        if (ToContainerIndex == 254)
                        {
                            toContainer = toContainer.Parent as Container;
                        }
                        
                        if (toContainer != null)
                        {
                            if (IsMoveable(context, fromItem, Count) && IsPickupable(context, fromItem) && IsPossible(context, fromItem, toContainer) )
                            {
                                context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toContainer, ToContainerIndex, Count, true) ).Then(ctx =>
                                {
                                    resolve(ctx);
                                } );
                            }
                        }
                    }
                }
            } );
        }
    }
}