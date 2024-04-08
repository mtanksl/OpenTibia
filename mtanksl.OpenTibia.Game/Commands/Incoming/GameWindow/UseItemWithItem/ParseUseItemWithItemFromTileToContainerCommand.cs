using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromTileToContainerCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, ushort fromTibiaId, byte toContainerId, byte toContainerIndex, ushort toTibiaId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            FromTibiaId = fromTibiaId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToTibiaId = toTibiaId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort FromTibiaId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Tile fromTile = Context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                if (Player.Tile.Position.CanSee(fromTile.Position) )
                {
                    Item fromItem = Player.Client.GetContent(fromTile, FromIndex) as Item;

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
                                    return Context.AddCommand(new PlayerUseItemWithItemCommand(this, Player, fromItem, toItem) );
                                }                            
                            }
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}