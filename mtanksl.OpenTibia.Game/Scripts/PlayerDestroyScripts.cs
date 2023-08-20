using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new PlayerDestroyContainerCloseHandler() );

            Context.Server.CommandHandlers.Add(new PlayerDestroyWindowCloseHandler() );

            Context.Server.CommandHandlers.Add(new PlayerDestroyChannelCloseHandler() );

            Context.Server.CommandHandlers.Add(new PlayerDestroyRuleViolationCloseHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}