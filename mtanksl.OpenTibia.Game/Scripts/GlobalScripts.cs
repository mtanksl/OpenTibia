using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        public override void Start(Server server)
        {
            server.QueueForExecution( () =>
            {
                return Promise.WhenAll(CreatureThink(), PlayerPing(), ClockTick() );
            } );
        }

        private async Promise CreatureThink()
        {
            while (true)
            {
                Context.AddEvent(new CreatureThinkGameEventArgs() );

                await Promise.Delay("CreatureThink", 100);
            }
        }

        private async Promise PlayerPing()
        {
            while (true)
            {
                Context.AddEvent(new PlayerPingGameEventArgs() );

                await Promise.Delay("PlayerPing", 60000);
            }
        }

        private async Promise ClockTick()
        {
            while (true)
            {
                Context.Server.Clock.Tick();

                Context.AddEvent(new ClockTickGameEventArgs(Context.Server.Clock.Hour, Context.Server.Clock.Minute) );

                await Promise.Delay("ClockTick", Clock.Interval);
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution("CreatureThink");

            server.CancelQueueForExecution("PlayerPing");

            server.CancelQueueForExecution("ClockTick");
        }
    }
}