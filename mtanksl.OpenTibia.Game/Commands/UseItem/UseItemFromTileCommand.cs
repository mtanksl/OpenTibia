using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromTileCommand : UseItemCommand
    {
        public UseItemFromTileCommand(Player player, Position fromPosition, byte fromIndex)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null)
                {
                    //Act

                    if (fromItem is Container container)
                    {
                        OpenOrCloseContainer(Player, container, server, context);
                    }
                }
            }            
        }
    }
}