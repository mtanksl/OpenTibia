using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class NpcTradeCommand : Command
    {
        public NpcTradeCommand(Npc npc, Player player, List<OfferDto> offers)
        {
            Npc = npc;

            Player = player;

            Offers = offers;
        }

        public Npc Npc { get; set; }

        public Player Player { get; set; }

        public List<OfferDto> Offers { get; set; }

        public override Promise Execute()
        {
            NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(Player);

            if (trading != null)
            {
                Context.Server.NpcTradings.RemoveTrading(trading);
            }

            trading = new NpcTrading()
            {
                OfferNpc = Npc,

                CounterOfferPlayer = Player,

                Offers = Offers
            };

            Context.Server.NpcTradings.AddTrading(trading);

            Context.AddPacket(Player, new InviteNpcTradeOutgoingPacket(Offers) );

            return Context.AddCommand(new NpcTradeUpdateStatsCommand(trading) );
        }
    }
}