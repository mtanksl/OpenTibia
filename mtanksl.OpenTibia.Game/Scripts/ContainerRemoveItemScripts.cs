using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ContainerRemoveItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new ContainerRemoveItemTradingRejectHandler() );       
            
            Context.Server.CommandHandlers.AddCommandHandler(new ContainerRemoveItemNpcTradingUpdateStatsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}