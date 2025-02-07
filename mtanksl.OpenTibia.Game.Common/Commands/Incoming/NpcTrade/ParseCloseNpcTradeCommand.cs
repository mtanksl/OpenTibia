using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;

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

                    PlayerCloseNpcTradeEventArgs e = new PlayerCloseNpcTradeEventArgs(Player);

                    ObserveEventArgs<PlayerCloseNpcTradeEventArgs> oe = ObserveEventArgs.Create(e);

                    Context.AddEvent(trading.OfferNpc, oe);

                    Context.AddEvent(Player, e);
                }
            }

            return Promise.Completed;
        }
    }
}