using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class ItemDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<ItemDestroyCommand>(new ItemDestroyContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<ItemDestroyCommand>(new ItemDestroyTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}