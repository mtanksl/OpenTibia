using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class TradeWithFromTileCommand : TradeWithCommand
    {
        public TradeWithFromTileCommand(Player player, Position fromPosition, byte fromIndex, uint creatureId)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ToCreatureId = creatureId;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public uint ToCreatureId { get; set; }

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

                    
                }
            }            
        }
    }
}