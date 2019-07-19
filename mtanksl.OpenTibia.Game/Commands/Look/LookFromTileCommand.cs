using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class LookFromTileCommand : Command
    {
        public LookFromTileCommand(Player player, Position fromPosition, byte fromIndex)
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

            

            //Act



            //Notify

            
        }
    }
}