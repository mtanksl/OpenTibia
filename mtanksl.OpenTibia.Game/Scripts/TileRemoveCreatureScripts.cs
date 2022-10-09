using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class TileRemoveCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.EventHandlers.Subscribe(new TileDepressHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}