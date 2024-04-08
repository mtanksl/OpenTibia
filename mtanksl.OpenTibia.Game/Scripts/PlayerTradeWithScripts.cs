using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerTradeWithScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new TradeWithCreatureWalkToSourceHandler() );
            
            Context.Server.CommandHandlers.AddCommandHandler(new TradeWithChestHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}