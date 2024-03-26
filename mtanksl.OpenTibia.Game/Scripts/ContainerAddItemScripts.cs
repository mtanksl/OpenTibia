using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ContainerAddItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new ContainerAddItemTradingRejectHandler() );            

            Context.Server.CommandHandlers.AddCommandHandler(new ContainerAddItemNpcTradingUpdateStatsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}