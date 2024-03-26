using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemMoveScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new ItemMoveContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ItemMoveTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}