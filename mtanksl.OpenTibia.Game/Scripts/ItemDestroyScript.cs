using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemDestroyScript : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new ItemDestroyContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ItemDestroyTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}