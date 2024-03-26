using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreatureDestroyTradingRejectHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    if (Context.Server.Tradings.Count > 0)
                    {
                        Trading trading = Context.Server.Tradings.GetTradingByOfferPlayer(player) ?? Context.Server.Tradings.GetTradingByCounterOfferPlayer(player);

                        if (trading != null)
                        {
                            Context.AddPacket(trading.OfferPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCanceled) );

                            Context.AddPacket(trading.OfferPlayer, new CloseTradeOutgoingPacket() );

                            Context.AddPacket(trading.CounterOfferPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCanceled) );

                            Context.AddPacket(trading.CounterOfferPlayer, new CloseTradeOutgoingPacket() );

                            Context.Server.Tradings.RemoveTrading(trading);
                        }
                    }

                    return Promise.Completed;
                } );
            }

            return next();            
        }
    }
}