using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerTradeWithScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new TradeWithCreatureWalkToSourceHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}