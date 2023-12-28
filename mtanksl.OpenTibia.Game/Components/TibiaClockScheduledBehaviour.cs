using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class TibiaClockScheduledBehaviour : Behaviour
    {
        private int hour;

        private int minute;

        public TibiaClockScheduledBehaviour(int hour, int minute)
        {
            this.hour = hour;
            
            this.minute = minute;
        }

        private Guid globalTibiaClockTick;

        public override void Start()
        {
            globalTibiaClockTick = Context.Server.EventHandlers.Subscribe<GlobalTibiaClockTickEventArgs>( (context, e) =>
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
            Context.Server.EventHandlers.Unsubscribe<GlobalTibiaClockTickEventArgs>(globalTibiaClockTick);
        }
    }
}