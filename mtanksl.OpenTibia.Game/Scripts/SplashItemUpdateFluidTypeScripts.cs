using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class SplashItemUpdateFluidTypeScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new SplashItemUpdateFluidTypeTradingRejectHandler() );            
        }

        public override void Stop()
        {
            
        }
    }
}