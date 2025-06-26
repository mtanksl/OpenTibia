using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseCancelOrRejectTradeCommand : IncomingCommand
    {
        public ParseCancelOrRejectTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Trading trading = Context.Server.Tradings.GetTradingByOfferPlayer(Player) ?? Context.Server.Tradings.GetTradingByCounterOfferPlayer(Player);

            if (trading != null)
            {
                Context.AddPacket(trading.OfferPlayer, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.TradeCanceled) );

                Context.AddPacket(trading.OfferPlayer, new CloseTradeOutgoingPacket() );

                Context.AddPacket(trading.CounterOfferPlayer, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.TradeCanceled) );

                Context.AddPacket(trading.CounterOfferPlayer, new CloseTradeOutgoingPacket() );

                Context.Server.Tradings.RemoveTrading(trading);
                                  
                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}