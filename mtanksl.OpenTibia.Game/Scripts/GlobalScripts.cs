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
                GlobalCreatures();
            
                GlobalItems();
            
                GlobalLight();

                return Promise.Completed();
            } );
        }

        private Promise GlobalCreatures()
        {
            Context context = Context.Current;

            return context.AddCommand(new GlobalCreaturesCommand() )

                .Then( () => Promise.Delay(context.Server, "Global_Creatures", 100) )

                .Then( () => GlobalCreatures() );
        }

        private Promise GlobalItems()
        {
            Context context = Context.Current;

            return context.AddCommand(new GlobalItemsCommand() )

                .Then( () => Promise.Delay(context.Server, "Global_Items", 60000) )
                
                .Then( () => GlobalItems() );
        }

        private Promise GlobalLight()
        {
            Context context = Context.Current;

            return context.AddCommand(new GlobalLightCommand() )

                .Then( () => Promise.Delay(context.Server, "Global_Light", Clock.Interval) )
                
                .Then( () => GlobalLight() );
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution("Global_Creatures");

            server.CancelQueueForExecution("Global_Items");

            server.CancelQueueForExecution("Global_Light");
        }
    }
}