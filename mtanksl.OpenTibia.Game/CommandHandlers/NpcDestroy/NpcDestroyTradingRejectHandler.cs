using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class NpcDestroyTradingRejectHandler : CommandHandler<NpcDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, NpcDestroyCommand command)
        {
            return next().Then( () =>
            {
                if (Context.Server.NpcTradings.Count > 0)
                {
                    foreach (var trading in Context.Server.NpcTradings.GetTradingByOfferNpc(command.Npc).ToList() )
                    {
                        Context.Server.NpcTradings.RemoveTrading(trading);

                        Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseNpcTradeOutgoingPacket() );
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}