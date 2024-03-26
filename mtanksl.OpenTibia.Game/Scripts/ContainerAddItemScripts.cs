using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ContainerAddItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new ContainerAddItemTradingRejectHandler() );            
        }

        public override void Stop()
        {
            
        }
    }
}