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

            Spawn();

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

        private void Tick(int index)
        {
            Promise.Delay("Tick", TimeSpan.FromMilliseconds(100) ).Then( () =>
            {
                Tick( (index + 1) % GlobalTickEventArgs.Instance.Length);

                Context.AddEvent(GlobalTickEventArgs.Instance[index] );

                return Promise.Completed;
            } );
        }

        private void Spawn()
        {
            Promise.Delay("Spawn", TimeSpan.FromSeconds(10) ).Then(() =>
            {
                Spawn();

                Context.AddEvent(GlobalSpawnEventArgs.Instance);

                return Promise.Completed;
            } );
        }
        
        private void Light()
        {
            Promise.Delay("Light", TimeSpan.FromSeconds(10) ).Then( () =>
            {
                Light();

                Context.AddEvent(GlobalEnvironmentLightEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        private void Ping()
        {
            Promise.Delay("Ping", TimeSpan.FromSeconds(10) ).Then( () =>
            {
                Ping();

                Context.AddEvent(GlobalPingEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.CancelQueueForExecution("RealClockTick");

            Context.Server.CancelQueueForExecution("TibiaClockTick");

            Context.Server.CancelQueueForExecution("Tick");

            Context.Server.CancelQueueForExecution("Spawn");

            Context.Server.CancelQueueForExecution("Light");

            Context.Server.CancelQueueForExecution("Ping");
        }
    }
}