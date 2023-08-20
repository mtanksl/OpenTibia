using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemDestroyScript : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new ItemDestroyContainerCloseHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}