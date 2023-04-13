using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromTileToContainerCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Tile fromTile = Context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                if (Player.Tile.Position.CanSee(fromTile.Position) )
                {
                    Item fromItem = fromTile.GetContent(FromIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                    {
                        Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                        if (toContainer != null)
                        {
                            if (IsMoveable(fromItem, Count) )
                            {
                                return Context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toContainer, ToContainerIndex, Count, true) );
                            }
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}