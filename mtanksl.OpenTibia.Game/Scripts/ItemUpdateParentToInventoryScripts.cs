using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemUpdateParentToInventoryScripts : IScript
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