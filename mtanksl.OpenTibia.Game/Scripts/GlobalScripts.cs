using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        private static DateTime GetNext(DateTime now, int millisecond)
        {
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0)
                .AddMilliseconds(Round(now.Second * 1000 + now.Millisecond, millisecond) )
                .AddMilliseconds(millisecond);
        }

        private static int Round(double value, int round)
        {
            return (int)Math.Floor(value / round) * round;
        }

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

            DateTime next = GetNext(now, 60 * 1000);

            Promise.Delay("RealClockTick", next - now).Then( () =>
            {
                RealClockTick();

                Context.AddEvent(new GlobalRealClockTickEventArgs(next.Hour, next.Minute) );

                return Promise.Completed;
            } );
        }

        private void TibiaClockTick()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNext(now, Clock.Interval);

            Promise.Delay("TibiaClockTick", next - now).Then( () =>
            {
                TibiaClockTick();

                Context.Server.Clock.Tick();

                Context.AddEvent(new GlobalTibiaClockTickEventArgs(Context.Server.Clock.Hour, Context.Server.Clock.Minute) );

                return Promise.Completed;
            } );
        }

        private void Tick(int index)
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNext(now, 100);

            Promise.Delay("Tick", next - now).Then( () =>
            {
                Tick( (index + 1) % GlobalTickEventArgs.Instance.Length);

                Context.AddEvent(GlobalTickEventArgs.Instance[index] );

                return Promise.Completed;
            } );
        }

        private void Spawn()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNext(now, 10 * 1000);

            Promise.Delay("Spawn", next - now).Then(() =>
            {
                Spawn();

                Context.AddEvent(GlobalSpawnEventArgs.Instance);

                return Promise.Completed;
            } );
        }
        
        private void Light()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNext(now, 10 * 1000);

            Promise.Delay("Light", next - now).Then( () =>
            {
                Light();

                Context.AddEvent(GlobalEnvironmentLightEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        private void Ping()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNext(now, 10 * 1000);

            Promise.Delay("Ping", next - now).Then( () =>
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