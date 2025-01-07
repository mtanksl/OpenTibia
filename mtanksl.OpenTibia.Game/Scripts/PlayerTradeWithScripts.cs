using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class PlayerTradeWithScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerTradeWithCommand>(new TradeWithChestHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}