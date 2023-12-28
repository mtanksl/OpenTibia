using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class RealClockScheduledBehaviour : Behaviour
    {
        private int hour;

        private int minute;

        public RealClockScheduledBehaviour(int hour, int minute)
        {
            this.hour = hour;
            
            this.minute = minute;
        }

        private Guid globalRealClockTick;

        public override void Start()
        {
            globalRealClockTick = Context.Server.EventHandlers.Subscribe<GlobalRealClockTickEventArgs>( (context, e) =>
            {
                if (e.Hour == hour && e.Minute == minute)
                {
                    return Update();
                }

                return Promise.Completed;
            } );
        }

        public abstract Promise Update();

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTibiaClockTickEventArgs>(globalRealClockTick);
        }
    }
}