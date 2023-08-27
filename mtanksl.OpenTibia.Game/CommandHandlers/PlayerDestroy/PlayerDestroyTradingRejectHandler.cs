using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyTradingRejectHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                Trading trading = Context.Server.Tradings.GetTradingByOfferPlayer(command.Player) ?? Context.Server.Tradings.GetTradingByCounterOfferPlayer(command.Player);

                if (trading != null)
                {
                    Context.AddPacket(trading.OfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCancelled) );

                    Context.AddPacket(trading.OfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                    Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TradeCancelled) );

                    Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                    Context.Server.Tradings.RemoveTrading(trading);
                }

                return Promise.Completed;
            } );
        }
    }
}