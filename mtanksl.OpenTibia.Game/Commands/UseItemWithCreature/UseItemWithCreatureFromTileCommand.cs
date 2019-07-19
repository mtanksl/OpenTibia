using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromTileCommand : Command
    {
        public UseItemWithCreatureFromTileCommand(Player player, Position fromPosition, byte fromIndex, uint creatureId)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public uint CreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act



            //Notify

            
        }
    }
}