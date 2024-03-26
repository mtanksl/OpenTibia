using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class FluidItemUpdateFluidTypeScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new FluidItemUpdateFluidTypeTradingRejectHandler() );            
        }

        public override void Stop()
        {
            
        }
    }
}