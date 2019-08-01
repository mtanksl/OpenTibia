using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Global
{
    public class GlobalScript : IScript
    {
        public void Start(Server server)
        {
            server.QueueForExecution(Constants.GlobalLightSchedulerEvent, Constants.GlobalLightSchedulerEventInterval, new GlobalLightCommand() );

            server.QueueForExecution(Constants.GlobalCreaturesSchedulerEvent, Constants.GlobalCreaturesSchedulerEventInterval, new GlobalCreaturesCommand() );
        }

        public void Stop(Server server)
        {
            server.CancelQueueForExecution(Constants.GlobalLightSchedulerEvent);

            server.CancelQueueForExecution(Constants.GlobalCreaturesSchedulerEvent);
        }
    }
}