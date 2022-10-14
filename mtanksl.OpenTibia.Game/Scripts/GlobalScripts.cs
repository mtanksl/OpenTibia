using OpenTibia.Common.Structures;
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

        private Promise GlobalCreatures(Context context)
        {
            return context.AddCommand(new GlobalCreaturesCommand() ).Then(ctx =>
            {
                return Promise.Delay(ctx.Server, "GlobalCreatures", 100);

            } ).Then(ctx =>
            {
                return GlobalCreatures(ctx);
            } );
        }

        private Promise GlobalItems(Context context)
        {
            return context.AddCommand(new GlobalItemsCommand() ).Then(ctx =>
            {
                return Promise.Delay(ctx.Server, "GlobalItems", 60000);

            } ).Then(ctx =>
            {
                return GlobalItems(ctx);
            } );
        }

        private Promise GlobalLight(Context context)
        {
            return context.AddCommand(new GlobalLightCommand() ).Then(ctx =>
            {
                return Promise.Delay(ctx.Server, "GlobalLight", Clock.Interval);

            } ).Then(ctx =>
            {
                return GlobalLight(ctx);
            } );
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution("GlobalCreatures");

            server.CancelQueueForExecution("GlobalItems");

            server.CancelQueueForExecution("GlobalLight");
        }
    }
}