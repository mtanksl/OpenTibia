using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerRotateItemScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new RotateItemTransformHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}