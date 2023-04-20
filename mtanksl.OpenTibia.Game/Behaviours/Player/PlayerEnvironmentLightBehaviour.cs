using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerEnvironmentLightBehaviour : Behaviour
    {
        public PlayerEnvironmentLightBehaviour()
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

            token = Context.Server.EventHandlers.Subscribe<GlobalClockTickEventArgs>( (context, e) =>
            {
                return Update();
            } );
        }

        private Promise Update()
        {
            Context.AddPacket(player.Client.Connection, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );

            return Promise.Completed;
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalClockTickEventArgs>(token);
        }
    }
}