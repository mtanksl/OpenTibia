using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class ContainerAddItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<ContainerAddItemCommand>(new ContainerAddItemTradingRejectHandler() );            

            Context.Server.CommandHandlers.AddCommandHandler<ContainerAddItemCommand>(new ContainerAddItemNpcTradingUpdateStatsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}