using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class RotateItemFromTileCommand : RotateItemCommand
    {
        public RotateItemFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.Rotatable) )
                    {
                        //Act

                        if ( IsNextTo(fromTile, server, context) )
                        {
                            RotateItem(fromItem, server, context);
                        }
                    }
                }
            }
        }
    }
}