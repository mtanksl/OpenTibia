using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class NpcDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new NpcDestroyTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}