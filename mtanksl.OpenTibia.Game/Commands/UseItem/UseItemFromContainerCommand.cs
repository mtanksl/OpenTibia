using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromContainerCommand : UseItemCommand
    {
        public UseItemFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte containerId)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ContainerId = containerId;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ContainerId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    //Act

                    Container container = fromItem as Container;

                    if (container != null)
                    {
                        if (ContainerId == FromContainerId)
                        {
                            ReplaceOrCloseContainer(Player, ContainerId, container, server, context);
                        }
                        else
                        {
                            OpenOrCloseContainer(Player, container, server, context);
                        }
                    }

                    base.Execute(server, context);
                }
            }
        }
    }
}