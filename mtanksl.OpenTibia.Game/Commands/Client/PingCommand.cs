using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Commands
{
    public class PingCommand : Command
    {
        private Server server;

        public PingCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange
            
            DateTime ping = DateTime.UtcNow;

            //Act

            Player.Client.Connection.Latency = ping;

            //Notify

            context.Response.Write(Player.Client.Connection, new Ping() );
        }
    }
}