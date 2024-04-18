using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromContainerToContainerCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromContainerToContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort fromTibiaId, byte toContainerId, byte toContainerIndex, ushort toTibiaId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            FromTibiaId = fromTibiaId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToTibiaId = toTibiaId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort FromTibiaId { get; set; }
        
        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.Containers.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromTibiaId)
                {
                    Container toContainer = Player.Client.Containers.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        Item toItem = toContainer.GetContent(ToContainerIndex) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToTibiaId)
                        {
                            if ( IsUseable(fromItem) )
                            {
                                return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                            }
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}