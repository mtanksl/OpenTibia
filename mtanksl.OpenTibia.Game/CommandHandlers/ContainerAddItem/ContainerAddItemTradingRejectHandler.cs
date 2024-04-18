using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerAddItemTradingRejectHandler : CommandHandler<ContainerAddItemCommand>
    {
        public override Promise Handle(Func<Promise> next, ContainerAddItemCommand command)
        {
            return next().Then( () =>
            {
                if (Context.Server.Tradings.Count > 0)
                {
                    RejectTrade(command.Container);
                }

                return Promise.Completed;
            } );
        }

        private void RejectTrade(Item item)
        {
            Trading trading = Context.Server.Tradings.GetTradingByOffer(item) ?? Context.Server.Tradings.GetTradingByCounterOffer(item);

            if (trading != null)
            {
                Context.AddPacket(trading.OfferPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCanceled) );

                Context.AddPacket(trading.OfferPlayer, new CloseTradeOutgoingPacket() );

                Context.AddPacket(trading.CounterOfferPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCanceled) );

                Context.AddPacket(trading.CounterOfferPlayer, new CloseTradeOutgoingPacket() );

                Context.Server.Tradings.RemoveTrading(trading);
            }
        }
    }
}