using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToContainerCommand : MoveItemCommand
    {
        public MoveItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, byte toContainerId, byte toContainerIndex, byte count)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToContainerId = ToContainerId;

            ToContainerIndex = ToContainerIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        //Act

                        RemoveItem(fromTile, fromItem, server, context);

                        AddItem(toContainer, fromItem, server, context);
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}