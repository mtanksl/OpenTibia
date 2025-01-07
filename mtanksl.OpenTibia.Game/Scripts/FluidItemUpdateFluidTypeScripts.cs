using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class FluidItemUpdateFluidTypeScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<FluidItemUpdateFluidTypeCommand>(new FluidItemUpdateFluidTypeTradingRejectHandler() );       
            
            Context.Server.CommandHandlers.AddCommandHandler<FluidItemUpdateFluidTypeCommand>(new FluidItemUpdateFluidTypeNpcTradingUpdateStatsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}