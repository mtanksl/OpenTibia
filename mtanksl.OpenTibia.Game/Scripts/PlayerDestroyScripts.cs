using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyWindowCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyChannelCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyRuleViolationCloseHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}