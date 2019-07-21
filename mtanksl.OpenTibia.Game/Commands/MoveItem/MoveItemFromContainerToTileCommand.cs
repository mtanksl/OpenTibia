using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToTileCommand : MoveItemCommand
    {
        public MoveItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, Position toPosition, byte count)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        //Act

                        Container parent = fromItem as Container;

                        if (parent != null)
                        {
                            if ( !Player.Tile.Position.IsNextTo(toTile.Position) )
                            {
                                CloseContainers(Player, parent, server, context);
                            }
                        }

                        RemoveItem(fromContainer, FromContainerIndex, server, context);

                        AddItem(toTile, fromItem, server, context);

                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}