using OpenTibia.Common.Objects;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Commands
{
    public class PongCommand : Command
    {
        private Server server;

        public PongCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            DateTime ping = Player.Client.Connection.Latency;

            DateTime pong = DateTime.UtcNow;

            //Act

            server.QueueForExecution(Constants.PlayerPingSchedulerEvent(Player), 10000, context, new PingCommand(server) { Player = Player }, null);
            
            //Notify
        }
    }
}