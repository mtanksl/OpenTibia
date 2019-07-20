using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class WalkCommand : Command
    {
        public WalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            Position fromPosition = fromTile.Position;

            Position toPosition = fromPosition.Offset(MoveDirection);

            Tile toTile = server.Map.GetTile(toPosition);

            if (toTile != null)
            {
                //Act

                TeleportCommand command = new TeleportCommand(Player, toTile);

                command.Completed += (s, e) =>
                {
                    OnCompleted(e);
                };

                server.QueueForExecution(Constants.PlayerWalkSchedulerEvent(Player), 1000 * fromTile.Ground.Metadata.Speed / Player.Speed, command);

                //Notify
            }
        }
    }
}