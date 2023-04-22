using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        public override void Start(Server server)
        {
            Tick(server);

            TibiaClockTick(server);

            Ping(server);
        }

        private void Tick(Server server)
        {
            Promise.Delay("Tick", TimeSpan.FromMilliseconds(100) ).Then( () =>
            {
                Tick(server);

                Context.AddEvent(new GlobalTickEventArgs() );

                return Promise.Completed;
            } );
        }

        private void TibiaClockTick(Server server)
        {
            Promise.Delay("TibiaClockTick", TimeSpan.FromMilliseconds(Clock.Interval) ).Then( () =>
            {
                TibiaClockTick(server);

                Context.Server.Clock.Tick();

                Context.AddEvent(new GlobalTibiaClockTickEventArgs(Context.Server.Clock.Hour, Context.Server.Clock.Minute) );

                return Promise.Completed;
            } );
        }

        private void Ping(Server server)
        {
            Promise.Delay("Ping", TimeSpan.FromSeconds(10) ).Then( () =>
            {
                Ping(server);

                Context.AddEvent(new GlobalPingEventArgs() );

                return Promise.Completed;
            } );
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution("Tick");

            server.CancelQueueForExecution("TibiaClockTick");

            server.CancelQueueForExecution("Ping");
        }
    }
}