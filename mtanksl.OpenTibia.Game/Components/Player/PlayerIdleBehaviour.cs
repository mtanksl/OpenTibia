using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerIdleBehaviour : Behaviour
    {
        private DateTime nextUse = DateTime.UtcNow;

        public void SetUse()
        {
            nextUse = DateTime.UtcNow.AddMilliseconds(200);

            lastAction = DateTime.UtcNow;
        }

        public bool CanUse(out TimeSpan executeIn)
        {
            if (DateTime.UtcNow > nextUse)
            {
                executeIn = TimeSpan.Zero;

                return true;
            }

            executeIn = nextUse - DateTime.UtcNow;

            return false;
        }

        private DateTime nextUseWith = DateTime.UtcNow;

        public void SetUseWith()
        {
            nextUseWith = DateTime.UtcNow.AddMilliseconds(1000);

            lastAction = DateTime.UtcNow;
        }

        public bool CanUseWith(out TimeSpan executeIn)
        {
            if (DateTime.UtcNow > nextUseWith)
            {
                executeIn = TimeSpan.Zero;

                return true;
            }

            executeIn = nextUseWith - DateTime.UtcNow;

            return false;
        }

        private DateTime nextWalk = DateTime.UtcNow;

        public void SetWalk(TimeSpan executeIn)
        {
            nextWalk = DateTime.UtcNow.Add(executeIn);

            lastAction = DateTime.UtcNow;
        }

        public bool CanWalk(out TimeSpan executeIn)
        {
            if (DateTime.UtcNow > nextWalk)
            {
                executeIn = TimeSpan.Zero;

                return true;
            }

            executeIn = nextWalk - DateTime.UtcNow;

            return false;
        }

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

                if (totalMinutes >= 16)
                {
                    return Context.AddCommand(new ParseLogOutCommand(player) );
                }
                else if (totalMinutes >= 15)
                {
                    Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "There was no variation in your behaviour for 15 minutes. You will be disconnected in one minute if there is no change in your actions until then.") );
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalRealClockTickEventArgs>(globalRealClockTick);
        }
    }
}