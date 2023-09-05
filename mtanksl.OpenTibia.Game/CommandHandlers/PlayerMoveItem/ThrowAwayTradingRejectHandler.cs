using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ThrowAwayTradingRejectHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (Context.Server.Tradings.Count > 0)
            {
                Reject(command.Item);
            }

            return next();
        }

        private void Reject(Item item)
        {
            Trading trading = Context.Server.Tradings.GetTradingByOffer(item) ?? Context.Server.Tradings.GetTradingByCounterOffer(item);

            if (trading != null)
            {
                Context.AddPacket(trading.OfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCanceled) );

                Context.AddPacket(trading.OfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCanceled) );

                Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                Context.Server.Tradings.RemoveTrading(trading);
            }

            if (item is Container container)
            {
                foreach (var child in container.GetItems() )
                {
                    Reject(child);
                }
            }
        }
    }
}