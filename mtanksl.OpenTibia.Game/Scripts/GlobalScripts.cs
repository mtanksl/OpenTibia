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

                await Promise.Delay("GlobalCreatures", 100);
            }
        }

        private async Promise GlobalItems()
        {
            while (true)
            {
                await Context.Current.AddCommand(new GlobalItemsCommand() );

                await Promise.Delay("GlobalItems", 60000);
            }
        }

        private async Promise GlobalLight()
        {
            while (true)
            {
                await Context.Current.AddCommand(new GlobalLightCommand() );

                await Promise.Delay("GlobalLight", Clock.Interval);
            }
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution("GlobalCreatures");

            server.CancelQueueForExecution("GlobalItems");

            server.CancelQueueForExecution("GlobalLight");
        }
    }
}