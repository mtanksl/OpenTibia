﻿using OpenTibia.Common.Objects;
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
            DateTime nextAction = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[GameObject.Id % GlobalTickEventArgs.Instance.Length], (context, e) =>
            {
                if (DateTime.UtcNow >= nextAction)
                {
                    if (count > 0)
                    {
                        count--;
                    }

                    nextAction = DateTime.UtcNow.AddMilliseconds(1500);
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