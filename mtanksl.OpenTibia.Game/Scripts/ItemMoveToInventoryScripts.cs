using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemMoveToInventoryScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new ItemMoveToInventoryContainerCloseHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}