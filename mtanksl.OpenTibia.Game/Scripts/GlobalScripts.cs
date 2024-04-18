using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        public override void Start()
        {
            RealClockTick();

            TibiaClockTick();

            Tick(0);

            Light();

            Ping();
        }

        private void RealClockTick()
        {
            DateTime now = DateTime.Now;

            DateTime next = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(1);

            TimeSpan diff = next - now;

            Promise.Delay("RealClockTick", diff).Then( () =>
            {
                RealClockTick();

                Context.AddEvent(new GlobalRealClockTickEventArgs(next.Hour, next.Minute) );

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

        //TODO: Improve performance

        private static GlobalTickEventArgs[] globalTickEventArgs = new GlobalTickEventArgs[10]
        {
            new GlobalTickEventArgs() { Index = 0 },
            new GlobalTickEventArgs() { Index = 1 },
            new GlobalTickEventArgs() { Index = 2 },
            new GlobalTickEventArgs() { Index = 3 },
            new GlobalTickEventArgs() { Index = 4 },
            new GlobalTickEventArgs() { Index = 5 },
            new GlobalTickEventArgs() { Index = 6 },
            new GlobalTickEventArgs() { Index = 7 },
            new GlobalTickEventArgs() { Index = 8 },
            new GlobalTickEventArgs() { Index = 9 }
        };

        private void Tick(int index)
        {
            Promise.Delay("Tick", TimeSpan.FromMilliseconds(100) ).Then( () =>
            {
                Tick( (index + 1) % 10 );

                Context.AddEvent(globalTickEventArgs[index] );

                return Promise.Completed;
            } );
        }

        private static GlobalEnvironmentLightEventArgs globalLightEventArgs = new GlobalEnvironmentLightEventArgs();

        private void Light()
        {
            Promise.Delay("Light", TimeSpan.FromSeconds(10) ).Then( () =>
            {
                Light();

                Context.AddEvent(globalLightEventArgs);

                return Promise.Completed;
            } );
        }

        private static GlobalPingEventArgs globalPingEventArgs = new GlobalPingEventArgs();

        private void Ping()
        {
            Promise.Delay("Ping", TimeSpan.FromSeconds(10) ).Then( () =>
            {
                Ping();

                Context.AddEvent(globalPingEventArgs);

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.CancelQueueForExecution("RealClockTick");

            Context.Server.CancelQueueForExecution("TibiaClockTick");

            Context.Server.CancelQueueForExecution("Tick");

            Context.Server.CancelQueueForExecution("Light");

            Context.Server.CancelQueueForExecution("Ping");
        }
    }
}