using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromContainerCommand : UseItemCommand
    {
        public UseItemFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerId) as Item;

                if (fromItem != null)
                {
                    //Act

                    Container container = fromItem as Container;

                    if (container != null)
                    {
                        OpenOrCloseContainer(Player, container, server, context);
                    }
                }
            }
        }
    }
}