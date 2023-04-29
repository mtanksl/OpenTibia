using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerPingBehaviour : Behaviour
    {
        private Guid token;

        private DateTime lastPingResponse = DateTime.UtcNow;

        public override void Start(Server server)
        {
            Player player = (Player)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalPingEventArgs>( (context, e) =>
            {
                if ( (DateTime.UtcNow - lastPingResponse).TotalMinutes > 1)
                {
                    return Context.AddCommand(new ParseLogOutCommand(player) );
                }

                Context.AddPacket(player.Client.Connection, new PingOutgoingPacket() );

                return Promise.Completed;
            } );
        }

        public void SetLastPingResponse()
        {
            lastPingResponse = DateTime.UtcNow;
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalPingEventArgs>(token);
        }
    }
}