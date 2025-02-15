using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemTransformTradingRejectHandler : CommandResultHandler<Item, ItemTransformCommand>
    {
        public override PromiseResult<Item> Handle(Func<PromiseResult<Item>> next, ItemTransformCommand command)
        {
            return next().Then( (item) =>
            {
                if (Context.Server.Tradings.Count > 0)
                {
                    RejectTrade(command.Item);
                }

                return Promise.FromResult(item);
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

            if (item is Container container)
            {
                foreach (var child in container.GetItems() )
                {
                    RejectTrade(child);
                }
            }
        }
    }
}