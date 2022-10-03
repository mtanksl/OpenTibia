using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemMoveToContainerScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new ItemMoveToContainerContainerCloseHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}