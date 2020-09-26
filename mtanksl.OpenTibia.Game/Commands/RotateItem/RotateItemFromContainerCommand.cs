using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class RotateItemFromContainerCommand : RotateItemCommand
    {
        public RotateItemFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Context context)
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    if ( IsRotatable(fromItem, context) )
                    {
                        RotateItem(fromItem, context);
                    }
                }
            }
        }
    }
}