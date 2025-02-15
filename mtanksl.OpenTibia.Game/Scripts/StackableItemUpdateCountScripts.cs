using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class StackableItemUpdateCountScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<StackableItemUpdateCountCommand>(new StackableItemUpdateCountTradingRejectHandler() );        
            
            Context.Server.CommandHandlers.AddCommandHandler<StackableItemUpdateCountCommand>(new StackableItemUpdateCountNpcTradingUpdateStatsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<StackableItemUpdateCountCommand>(new StackableItemUpdateCountUpdatePlayerCapacitytHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}