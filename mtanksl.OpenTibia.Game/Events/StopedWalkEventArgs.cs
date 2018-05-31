using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Events
{
    public class StopedWalkEventArgs : EventArgs, IEvent
    {
        private Server server;

        public StopedWalkEventArgs(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public void Execute(Context context)
        {
            context.Response.Write(Player.Client.Connection, new StopWalk(Player.Direction) );
        }
    }
}