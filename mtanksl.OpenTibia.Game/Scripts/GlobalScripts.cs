using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : IScript
    {
        public void Start(Server server)
        {
            server.QueueForExecution( () =>
            {
                return Promise.WhenAll(GlobalCreatures(), GlobalItems(), GlobalLight() );
            } );
        }

        private async Promise GlobalCreatures()
        {
            while (true)
            {
                await Context.Current.AddCommand(new GlobalCreaturesCommand() );

                await Promise.Delay("Global_Creatures", 100);
            }
        }

        private async Promise GlobalItems()
        {
            while (true)
            {
                await Context.Current.AddCommand(new GlobalItemsCommand() );

                await Promise.Delay("Global_Items", 60000);
            }
        }

        private async Promise GlobalLight()
        {
            while (true)
            {
                await Context.Current.AddCommand(new GlobalLightCommand() );

                await Promise.Delay("Global_Light", Clock.Interval);
            }
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution("Global_Creatures");

            server.CancelQueueForExecution("Global_Items");

            server.CancelQueueForExecution("Global_Light");
        }
    }
}