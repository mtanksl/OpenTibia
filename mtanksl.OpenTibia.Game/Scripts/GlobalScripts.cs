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
            RealClockTick();

            TibiaClockTick();

            Light();

            Ping();

            Tick();
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

        private static GlobalLightEventArgs globalLightEventArgs = new GlobalLightEventArgs();

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

        private static GlobalTickEventArgs globalTickEventArgs = new GlobalTickEventArgs();

        private void Tick()
        {
            Promise.Delay("Tick", TimeSpan.FromSeconds(1) ).Then( () =>
            {
                Tick();

                Context.AddEvent(globalTickEventArgs);

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.CancelQueueForExecution("RealClockTick");

            Context.Server.CancelQueueForExecution("TibiaClockTick");

            Context.Server.CancelQueueForExecution("Light");

            Context.Server.CancelQueueForExecution("Ping");

            Context.Server.CancelQueueForExecution("Tick");
        }
    }
}