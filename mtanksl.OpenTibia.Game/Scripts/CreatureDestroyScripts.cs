using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new CreatureDestroyTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}