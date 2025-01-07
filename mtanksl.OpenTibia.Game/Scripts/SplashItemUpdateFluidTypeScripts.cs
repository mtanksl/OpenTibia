using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class SplashItemUpdateFluidTypeScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<SplashItemUpdateFluidTypeCommand>(new SplashItemUpdateFluidTypeTradingRejectHandler() );    
            
            Context.Server.CommandHandlers.AddCommandHandler<SplashItemUpdateFluidTypeCommand>(new SplashItemUpdateFluidTypeNpcTradingUpdateStatsHandler() );            
        }

        public override void Stop()
        {
            
        }
    }
}