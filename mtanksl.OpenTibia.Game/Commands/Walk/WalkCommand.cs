using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

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

            //Act

            server.QueueForExecution(Constants.PlayerWalkSchedulerEvent(Player), 1000 * fromTile.Ground.Metadata.Speed / Player.Speed, new TeleportCommand(Player, toPosition), OnCompleted);

            //Notify
        }

        public EventHandler Completed;

        protected virtual void OnCompleted()
        {
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }
    }
}