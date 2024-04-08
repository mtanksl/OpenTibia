using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseNpcTradeCommand : IncomingCommand
    {
        public ParseCloseNpcTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GameplayPrivateNpcSystem)
            {
                NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    Context.Server.NpcTradings.RemoveTrading(trading);

                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>(trading.OfferNpc);

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.CloseNpcTrade(Player);
                    }
                }
            }

            return Promise.Completed;
        }
    }
}