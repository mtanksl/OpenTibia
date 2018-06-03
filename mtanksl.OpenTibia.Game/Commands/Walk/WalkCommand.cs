using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Commands
{
    public class WalkCommand : Command
    {
        private Server server;

        public WalkCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }
        
        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            Position fromPosition = fromTile.Position;

            Position toPosition = fromPosition.Offset(MoveDirection);

            int delay = 1000 * fromTile.Ground.Metadata.Speed / Player.Speed;

            //Act

            server.QueueForExecution(Constants.PlayerWalkSchedulerEvent(Player), delay, context, new TeleportCommand(server) { Player = Player, Position = toPosition }, OnCompleted);

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