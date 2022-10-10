using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MoveCreatureWalkToSourceHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}