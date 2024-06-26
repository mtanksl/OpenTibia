﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerEnvironmentLightBehaviour : Behaviour
    {
        private Guid globalTibiaClockTick;

        public override void Start()
        {
            Player player = (Player)GameObject;

            globalTibiaClockTick = Context.Server.EventHandlers.Subscribe<GlobalEnvironmentLightEventArgs>( (context, e) =>
            {
                Context.AddPacket(player, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTibiaClockTick);
        }
    }
}