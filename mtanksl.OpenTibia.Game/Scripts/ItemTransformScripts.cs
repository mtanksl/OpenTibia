using OpenTibia.Common.Objects;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class ItemTransformScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<Item, ItemTransformCommand>(new ItemTransformContainerCloseHandler() ); 
            
            Context.Server.CommandHandlers.AddCommandHandler<Item, ItemTransformCommand>(new ItemTransformTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}