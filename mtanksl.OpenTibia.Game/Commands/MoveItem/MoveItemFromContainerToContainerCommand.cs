using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToContainerCommand : MoveItemCommand
    {
        public MoveItemFromContainerToContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
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

        public override void Execute(Context context)
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        if ( IsMoveable(fromItem, context) &&

                             IsPickupable(fromItem, context) &&

                             IsPossible(fromItem, toContainer, context) && 
                             
                             IsEnoughtSpace(fromItem, toContainer, context) )
                        {
                            MoveItem(fromItem, toContainer, ToContainerIndex, Count, context);
                        }
                    }
                }
            }
        }
    }
}