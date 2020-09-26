using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromContainerToContainerCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromContainerToContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort fromItemId, byte toContainerId, byte toContainerIndex, ushort toItemId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            FromItemId = fromItemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToItemId = toItemId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort FromItemId { get; set; }
        
        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Context context)
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        Item toItem = toContainer.GetContent(ToContainerIndex) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                        {
                            if ( IsUseable(fromItem, context) )
                            {
                                UseItemWithItem(fromItem, toItem, context);
                            }
                        }
                    }
                }
            }
        }        
    }
}