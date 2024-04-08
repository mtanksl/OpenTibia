using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerTradeWithScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new TradeWithCreatureWalkToSourceHandler() ); //TODO: Re-validate rules for incoming packet
            
            Context.Server.CommandHandlers.AddCommandHandler(new TradeWithChestHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}