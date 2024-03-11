using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseNpcTradeCommand : Command
    {
        public ParseCloseNpcTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GamePrivateNpcSystem)
            {
                NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    Context.Server.NpcTradings.RemoveTrading(trading);

                    //TODO: onclosenpctrade
                }
            }

            return Promise.Completed;
        }
    }
}