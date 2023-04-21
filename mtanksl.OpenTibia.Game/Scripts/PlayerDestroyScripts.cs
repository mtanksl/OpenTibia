using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerDestroyScripts : Script
    {
        public override void Start(Server server)
        {
            server.CommandHandlers.Add(new PlayerDestroyContainerCloseHandler() );

            server.CommandHandlers.Add(new PlayerDestroyWindowCloseHandler() );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}