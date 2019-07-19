using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToTileCommand : MoveItemCommand
    {
        public MoveItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, Position toPosition, byte count)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ToPosition = toPosition;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        //Act

                        RemoveItem(fromContainer, fromItem, server, context);

                        AddItem(toTile, fromItem, server, context);
                    }
                }
            }
        }
    }
}