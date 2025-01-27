using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class LogoutBlockCondition : Condition
    {
        public LogoutBlockCondition(TimeSpan duration) : base(ConditionSpecialCondition.LogoutBlock)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            Player player = (Player)creature;

            return Promise.Delay(key, Duration).Then( () =>
            {
                player.Combat.Clear();

                foreach (var observer in Context.Current.Server.Map.GetObserversOfTypePlayer(player.Tile.Position) )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(player, out clientIndex) )
                    {
                        Context.Current.AddPacket(observer, new SetSkullIconOutgoingPacket(player.Id, observer.Client.GetSkullIcon(player) ) );
                    }
                }

                return Promise.Completed;
            } );
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }

        public override Promise OnStop(Creature creature)
        {
            return Promise.Completed;
        }
    }
}