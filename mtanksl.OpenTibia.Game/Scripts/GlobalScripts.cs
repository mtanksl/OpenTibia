using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        public override void Start(Server server)
        {
            Tick(server);

            TibiaClockTick(server);

            PlayerPing(server);
        }

        private void Tick(Server server)
        {
            Promise.Delay("Tick", 100).Then( () =>
            {
                Tick(server);

                Context.AddEvent(new GlobalTickEventArgs() );

                return Promise.Completed;
            } );
        }

        private void TibiaClockTick(Server server)
        {
            Promise.Delay("TibiaClockTick", Clock.Interval).Then( () =>
            {
                TibiaClockTick(server);

                Context.Server.Clock.Tick();

                Context.AddEvent(new GlobalTibiaClockTickEventArgs(Context.Server.Clock.Hour, Context.Server.Clock.Minute) );

                return Promise.Completed;
            } );
        }

        private void PlayerPing(Server server)
        {
            Promise.Delay("PlayerPing", 60000).Then( () =>
            {
                PlayerPing(server);

                Context.AddEvent(new GlobalPlayerPingEventArgs() );

                return Promise.Completed;
            } );
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution("Tick");

            server.CancelQueueForExecution("TibiaClockTick");

            server.CancelQueueForExecution("PlayerPing");
        }
    }
}