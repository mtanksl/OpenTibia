using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToTileCommand : MoveItemCommand
    {
        public MoveItemFromTileToTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, Position toPosition, byte count) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

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
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        //Act

                        if ( IsMoveable(fromItem, server, context) && 
                            
                             IsNextTo(fromTile, server, context) && 
                             
                             CanThrow(fromTile, toTile, server, context) )
                        {
                            Tile nextTile = server.Map.GetNextTile(toTile);

                            if (nextTile == null)
                            {
                                nextTile = toTile;
                            }

                            new TileRemoveItemCommand(fromTile, FromIndex).Execute(server, context);

                            new TileAddItemCommand(nextTile, fromItem).Execute(server, context);

                            base.Execute(server, context);
                        }
                    }
                }
            }
        }
    }
}