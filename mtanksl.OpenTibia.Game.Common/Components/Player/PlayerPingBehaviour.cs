using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerPingBehaviour : Behaviour
    {
        private bool waitingPingResponse = false;

        private DateTime lastPingRequest = DateTime.UtcNow;

        private DateTime lastPingResponse = DateTime.UtcNow;

        public void SetLastPingResponse()
        {
            if (waitingPingResponse)
            {
                waitingPingResponse = false;

                lastPingResponse = DateTime.UtcNow;
            }
        }

        public int GetLatency()
        {
            return (int)(lastPingResponse - lastPingRequest).TotalMilliseconds;
        }

        private Guid globalPing;

        public override void Start()
        {
            Player player = (Player)GameObject;

            globalPing = Context.Server.EventHandlers.Subscribe<GlobalPingEventArgs>( (context, e) =>
            {
                var totalMinutes = (DateTime.UtcNow - lastPingResponse).TotalMinutes;

                if (totalMinutes >= Context.Server.Config.GameplayKickLostConnectionAfterMinutes)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureDestroyCommand(player) );
                    } );
                }
                else
                {
                    waitingPingResponse = true;

                    lastPingRequest = DateTime.UtcNow;

                    if (Context.Server.Features.HasFeatureFlag(FeatureFlag.ClientPing) )
			        {
                        Context.AddPacket(player, new PingRequestOutgoingPacket() );
                    }
                    else
                    {
                        Context.AddPacket(player, new PingResponseOutgoingPacket() );
                    }
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalPing);
        }
    }
}