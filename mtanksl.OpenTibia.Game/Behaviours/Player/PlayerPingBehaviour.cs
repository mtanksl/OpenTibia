using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerPingBehaviour : Behaviour
    {
        public PlayerPingBehaviour()
        {
            
        }

        public override bool IsUnique
        {
            get
            {
                return true;
            }
        }

       
        private Player player;

        private Guid token;

        public override void Start(Server server)
        {
            player = (Player)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalPingEventArgs>( (context, e) =>
            {
                return Update();
            } );
        }

        private DateTime lastPingResponse = DateTime.UtcNow;

        public void SetLastPingResponse()
        {
            lastPingResponse = DateTime.UtcNow;
        }

        private Promise Update()
        {
            if ( (DateTime.UtcNow - lastPingResponse).TotalMinutes > 1)
            {
                return Context.AddCommand(new ParseLogOutCommand(player) );
            }
            else
            {
                Context.AddPacket(player.Client.Connection, new PingOutgoingPacket() );

                return Promise.Completed;
            }
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalPingEventArgs>(token);
        }
    }
}