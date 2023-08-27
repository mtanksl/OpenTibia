using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseCancelOrRejectTradeCommand : Command
    {
        public ParseCancelOrRejectTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Trading trading = Context.Server.Tradings.GetTradingByOfferPlayer(Player);

            if (trading != null)
            {
                Context.AddPacket(trading.OfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCancelled) );

                Context.AddPacket(trading.OfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCancelled) );

                Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                Context.Server.Tradings.RemoveTrading(trading);
                                  
                return Promise.Completed;
            }
            else
            {
                trading = Context.Server.Tradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    Context.AddPacket(trading.OfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCancelled) );

                    Context.AddPacket(trading.OfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                    Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCancelled) );

                    Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                    Context.Server.Tradings.RemoveTrading(trading);

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}