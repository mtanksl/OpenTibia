using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerMuteBehaviour : Behaviour
    {
        private int count;

        private int muted;

        private DateTime mutedUntil;

        public bool IsMuted(out string message)
        {
            if (DateTime.UtcNow < mutedUntil)
            {
                message = "You are still muted for " + (int)Math.Ceiling( (mutedUntil - DateTime.UtcNow).TotalSeconds) + " seconds.";

                return true;
            }

            count++;

            if (count > 4)
            {
                muted++;

                mutedUntil = DateTime.UtcNow.AddSeconds(5 * muted * muted);

                message = "You are muted for " + (int)Math.Ceiling( (mutedUntil - DateTime.UtcNow).TotalSeconds) + " seconds.";

                return true;
            }

            message = null;

            return false;
        }

        private Guid globalTick;

        public override void Start()
        {
            int ticks = 1500;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(GameObject.Id), (context, e) =>
            {
                ticks -= e.Ticks;

                while (ticks <= 0)
                {
                    ticks += 1500;

                    if (count > 0)
                    {
                        count--;
                    }
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}