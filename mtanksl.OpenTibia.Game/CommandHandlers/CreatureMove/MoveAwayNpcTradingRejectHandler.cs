using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveAwayNpcTradingRejectHandler : CommandHandler<CreatureMoveCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureMoveCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(player);

                        if (trading != null)
                        {
                            if ( !trading.CounterOfferPlayer.Tile.Position.IsInRange(trading.OfferNpc.Tile.Position, 3) )
                            {
                                Context.Server.NpcTradings.RemoveTrading(trading);

                                Context.AddPacket(trading.CounterOfferPlayer, new CloseNpcTradeOutgoingPacket() );
                            }
                        }
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}