using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerPingBehaviour : Behaviour
    {
        private DateTime lastPingRequest = DateTime.UtcNow;

        private DateTime lastPingResponse = DateTime.UtcNow;

        public void SetLastPingResponse()
        {
            DateTime now = DateTime.UtcNow;

            if (now > lastPingRequest)
            {
                lastPingResponse = now;
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

                if (totalMinutes >= 1)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureDestroyCommand(player) );
                    } );
                }
                else
                {
                    lastPingRequest = DateTime.UtcNow;

                    Context.AddPacket(player, new PingOutgoingPacket() );
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalPingEventArgs>(globalPing);
        }
    }
}