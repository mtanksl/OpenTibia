using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        public override void Start()
        {
            Tick();

            TibiaClockTick();

            Ping();
        }

        private void Tick()
        {
            Promise.Delay("Tick", TimeSpan.FromMilliseconds(100) ).Then( () =>
            {
                Tick();

                Context.AddEvent(new GlobalTickEventArgs() );

                return Promise.Completed;
            } );
        }

        private void TibiaClockTick()
        {
            Promise.Delay("TibiaClockTick", TimeSpan.FromMilliseconds(Clock.Interval) ).Then( () =>
            {
                TibiaClockTick();

                Context.Server.Clock.Tick();

                Context.AddEvent(new GlobalTibiaClockTickEventArgs(Context.Server.Clock.Hour, Context.Server.Clock.Minute) );

                return Promise.Completed;
            } );
        }

        private void Ping()
        {
            Promise.Delay("Ping", TimeSpan.FromSeconds(10) ).Then( () =>
            {
                Ping();

                Context.AddEvent(new GlobalPingEventArgs() );

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.CancelQueueForExecution("Tick");

            Context.Server.CancelQueueForExecution("TibiaClockTick");

            Context.Server.CancelQueueForExecution("Ping");
        }
    }
}