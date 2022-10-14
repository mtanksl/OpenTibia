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

        private void GlobalCreatures(Context context)
        {
            Promise.Delay(context.Server, "GlobalCreatures", 1000).Then(ctx =>
            {
                return ctx.AddCommand(new GlobalCreaturesCommand() );

            } ).Then(ctx =>
            {
                GlobalCreatures(ctx);
            } );
        }

        private void GlobalItems(Context context)
        {
            Promise.Delay(context.Server, "GlobalItems", 60000).Then(ctx =>
            {
                return ctx.AddCommand(new GlobalItemsCommand() );

            } ).Then(ctx =>
            {
                GlobalItems(ctx);
            } );
        }

        private void GlobalLight(Context context)
        {
            Promise.Delay(context.Server, "GlobalLight", Clock.Interval).Then(ctx =>
            {
                return ctx.AddCommand(new GlobalLightCommand() );

            } ).Then(ctx =>
            {
                GlobalLight(ctx);
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