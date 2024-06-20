using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerIdleBehaviour : Behaviour
    {
        private DateTime lastAction = DateTime.UtcNow;

        public void SetLastAction()
        {
            lastAction = DateTime.UtcNow;
        }

        private Guid globalRealClockTick;

        public override void Start()
        {
            Player player = (Player)GameObject;

            globalRealClockTick = Context.Server.EventHandlers.Subscribe<GlobalRealClockTickEventArgs>( (context, e) =>
            {
                var totalMinutes = (DateTime.UtcNow - lastAction).TotalMinutes;

                if (totalMinutes >= Context.Server.Config.GameplayKickIdlePlayerAfterMinutes + 1)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureDestroyCommand(player) );
                    } );
                }
                else if (totalMinutes >= Context.Server.Config.GameplayKickIdlePlayerAfterMinutes)
                {
                    Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "There was no variation in your behaviour for 15 minutes. You will be disconnected in one minute if there is no change in your actions until then.") );
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalRealClockTick);
        }
    }
}