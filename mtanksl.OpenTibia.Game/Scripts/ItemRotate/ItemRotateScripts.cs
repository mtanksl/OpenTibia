using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemRotateScripts : IScript
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