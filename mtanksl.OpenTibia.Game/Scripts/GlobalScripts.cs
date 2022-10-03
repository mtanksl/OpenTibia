using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : IScript
    {
        public void Start(Server server)
        {
            server.QueueForExecution(ctx =>
            {
                GlobalCreatures(ctx);

                GlobalItems(ctx);

                GlobalLight(ctx);
            } );
        }

        private void GlobalCreatures(Context context)
        {
            context.AddCommand(new GlobalCreaturesCommand() ).Then(ctx =>
            {
                return Promise.Delay(ctx, Constants.GlobalCreaturesSchedulerEvent, Constants.GlobalCreaturesSchedulerEventInterval);

            } ) .Then(ctx =>
            {
                GlobalCreatures(ctx);
            } );
        }

        private void GlobalItems(Context context)
        {
            context.AddCommand(new GlobalItemsCommand() ).Then(ctx =>
            {
                return Promise.Delay(ctx, Constants.GlobalItemsSchedulerEvent, Constants.GlobalItemsSchedulerEventInterval);

            } ) .Then(ctx =>
            {
                GlobalItems(ctx);
            } );
        }

        private void GlobalLight(Context context)
        {
            context.AddCommand(new GlobalLightCommand() ).Then(ctx =>
            {
                return Promise.Delay(ctx, Constants.GlobalLightSchedulerEvent, Constants.GlobalLightSchedulerEventInterval);

            } ) .Then(ctx =>
            {
                GlobalLight(ctx);
            } );
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution(Constants.GlobalLightSchedulerEvent);

            server.CancelQueueForExecution(Constants.GlobalItemsSchedulerEvent);

            server.CancelQueueForExecution(Constants.GlobalCreaturesSchedulerEvent);
        }
    }
}