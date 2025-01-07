using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class ItemMoveScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<ItemMoveCommand>(new ItemMoveContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<ItemMoveCommand>(new ItemMoveTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}