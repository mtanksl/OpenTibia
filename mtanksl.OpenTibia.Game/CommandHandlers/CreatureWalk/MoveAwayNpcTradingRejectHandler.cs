using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveAwayNpcTradingRejectHandler : CommandHandler<CreatureWalkCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureWalkCommand command)
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

                                Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseNpcTradeOutgoingPacket() );
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