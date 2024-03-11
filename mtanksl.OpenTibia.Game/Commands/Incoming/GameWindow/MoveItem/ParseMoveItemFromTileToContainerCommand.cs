using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromTileToContainerCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, ushort tibiaId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            TibiaId = tibiaId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort TibiaId { get; set; }

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
                    Item fromItem = Player.Client.GetContent(fromTile, FromIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
                    {
                        Container toContainer = Player.Client.Containers.GetContainer(ToContainerId);

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