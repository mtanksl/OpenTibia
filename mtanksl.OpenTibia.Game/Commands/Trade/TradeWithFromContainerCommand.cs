using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TradeWithFromContainerCommand : TradeWithCommand
    {
        public TradeWithFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, uint creatureId)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ToCreatureId = creatureId;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public uint ToCreatureId { get; set; }

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

                    
                }
            }
        }
    }
}