using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : IScript
    {
        public void Start(Server server)
        {
            server.QueueForExecution(ctx =>
            {
                ctx.AddCommand(new GlobalLightCommand() );
            
                ctx.AddCommand(new GlobalItemsCommand() );
                        
                ctx.AddCommand(new GlobalCreaturesCommand() );
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