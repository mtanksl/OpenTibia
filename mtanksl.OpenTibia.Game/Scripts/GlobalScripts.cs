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

            return context.AddCommand(new GlobalCreaturesCommand() ).Then( () =>
            {
                return Promise.Delay(context.Server, "Global_Creatures", 100);

            } ).Then( () =>
            {
                return GlobalCreatures();
            } );
        }

        private Promise GlobalItems()
        {
            Context context = Context.Current;

            return context.AddCommand(new GlobalItemsCommand() ).Then( () =>
            {
                return Promise.Delay(context.Server, "Global_Items", 60000);

            } ).Then( () =>
            {
                return GlobalItems();
            } );
        }

        private Promise GlobalLight()
        {
            Context context = Context.Current;

            return context.AddCommand(new GlobalLightCommand() ).Then( () =>
            {
                return Promise.Delay(context.Server, "Global_Light", Clock.Interval);

            } ).Then( () =>
            {
                return GlobalLight();
            } );
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution("Global_Creatures");

            server.CancelQueueForExecution("Global_Items");

            server.CancelQueueForExecution("Global_Light");
        }
    }
}