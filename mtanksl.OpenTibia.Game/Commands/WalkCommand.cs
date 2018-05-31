using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class WalkCommand : AsyncCommand
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

            Tile toTile = server.Map.GetTile(toPosition);
            
            //Act

            Client client = (Client)Player.Client;

            if (client.Walking != null)
            {
                if ( client.Walking.Cancel() )
                {
                    AddEvent(new StopedWalkEventArgs(server)
                    {
                        Player = Player
                    } );
                }
            }

            client.Walking = server.Scheduler.QueueForExecution(1000, () =>
            {
                server.CommandBus.Execute(new TeleportCommand(server)
                {
                    Player = Player,

                    FromTile = fromTile,

                    ToTile = toTile

                }, context);

                OnComplete();
            } );
        }
    }
}