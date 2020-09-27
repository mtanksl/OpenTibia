using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithItemScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new RopeHandler() );

            server.CommandHandlers.Add(new ShovelHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}