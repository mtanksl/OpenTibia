using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerDestroyScripts : Script
    {
        public override void Start(Server server)
        {
            server.CommandHandlers.Add(new PlayerDestroyContainerCloseHandler() );

            server.CommandHandlers.Add(new PlayerDestroyWindowCloseHandler() );

            server.CommandHandlers.Add(new PlayerDestroyChannelCloseHandler() );

            server.CommandHandlers.Add(new PlayerDestroyRuleViolationCloseHandler() );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}