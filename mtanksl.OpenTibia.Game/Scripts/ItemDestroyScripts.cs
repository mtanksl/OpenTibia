using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemDestroyScripts : Script
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