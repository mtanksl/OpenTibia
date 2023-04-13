using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromContainerToTileCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, Position toPosition, byte count) :base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Tile toTile = Context.Server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        if (IsMoveable(fromItem, Count) )
                        {
                            return Context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toTile, 0, Count, true) );
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}