using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerTradeWithScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new TradeWithCreatureWalkToSourceHandler() );

            //TODO: You cannot use there.

            Context.Server.CommandHandlers.AddCommandHandler(new TradeWithCreatureWalkToTargetHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}