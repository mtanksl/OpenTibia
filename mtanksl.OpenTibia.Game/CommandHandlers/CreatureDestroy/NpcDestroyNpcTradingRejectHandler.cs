using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class NpcDestroyNpcTradingRejectHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Npc npc)
            {
                return next().Then( () =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        foreach (var trading in Context.Server.NpcTradings.GetTradingByOfferNpc(npc).ToArray() )
                        {
                            Context.Server.NpcTradings.RemoveTrading(trading);

                            Context.AddPacket(trading.CounterOfferPlayer, new CloseNpcTradeOutgoingPacket() );
                        }
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}