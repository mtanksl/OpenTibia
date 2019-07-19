using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToTileCommand : MoveItemCommand
    {
        public MoveItemFromTileToTileCommand(Player player, Position fromPosition, byte fromIndex, Position toPosition, byte count)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ToPosition = toPosition;

            Count = count;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        //Act

                        RemoveItem(fromTile, fromItem, server, context);

                        AddItem(toTile, fromItem, server, context);
                    }
                }
            }
        }        
    }
}