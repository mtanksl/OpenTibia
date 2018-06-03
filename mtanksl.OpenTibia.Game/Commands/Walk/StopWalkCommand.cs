using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class StopWalkCommand : Command
    {
        private Server server;

        public StopWalkCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }
                
        public override void Execute(Context context)
        {
            //Arrange

            //Act

            if (server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) ) )
            {
                //Notify

                context.Response.Write(Player.Client.Connection, new StopWalk(Player.Direction) );
            }
        }
    }
}