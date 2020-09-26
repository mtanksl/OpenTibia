using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Global
{
    public class GlobalScripts : IScript
    {
        public void Start(Server server)
        {
            var globalLightCommand = new GlobalLightCommand();

            globalLightCommand.Completed += (s, e) =>
            {
                e.Context.Server.QueueForExecution(Constants.GlobalLightSchedulerEvent, Constants.GlobalLightSchedulerEventInterval, globalLightCommand);
            };

            server.QueueForExecution(Constants.GlobalLightSchedulerEvent, Constants.GlobalLightSchedulerEventInterval, globalLightCommand);


            var globalItemsCommand = new GlobalItemsCommand();

            globalItemsCommand.Completed += (s, e) =>
            {
                e.Context.Server.QueueForExecution(Constants.GlobalItemsSchedulerEvent, Constants.GlobalItemsSchedulerEventInterval, globalItemsCommand);
            };

            server.QueueForExecution(Constants.GlobalItemsSchedulerEvent, Constants.GlobalItemsSchedulerEventInterval, globalItemsCommand );


            var globalCreaturesCommand = new GlobalCreaturesCommand();

            globalCreaturesCommand.Completed += (s, e) =>
            {
                e.Context.Server.QueueForExecution(Constants.GlobalCreaturesSchedulerEvent, Constants.GlobalCreaturesSchedulerEventInterval, globalCreaturesCommand);
            };

            server.QueueForExecution(Constants.GlobalCreaturesSchedulerEvent, Constants.GlobalCreaturesSchedulerEventInterval, globalCreaturesCommand);
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution(Constants.GlobalLightSchedulerEvent);

            server.CancelQueueForExecution(Constants.GlobalItemsSchedulerEvent);

            server.CancelQueueForExecution(Constants.GlobalCreaturesSchedulerEvent);
        }
    }
}