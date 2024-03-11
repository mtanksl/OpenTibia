using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class NpcTradeCommand : Command
    {
        public NpcTradeCommand(Npc npc, Player player, List<OfferDto> offers, List<CounterOfferDto> counterOffers, int money)
        {
            Npc = npc;

            Player = player;

            Offers = offers;

            CounterOffers = counterOffers;

            Money = money;
        }

        public Npc Npc { get; set; }

        public Player Player { get; set; }

        public List<OfferDto> Offers { get; set; }

        public List<CounterOfferDto> CounterOffers { get; set; }

        public int Money { get; set; }

        public override Promise Execute()
        {
            NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(Player);

            if (trading != null)
            {
                Context.Server.NpcTradings.RemoveTrading(trading); //TODO: Destroy
            }

            trading = new NpcTrading()
            {
                OfferNpc = Npc,

                CounterOfferPlayer = Player,

                Offers = Offers
            };

            Context.Server.NpcTradings.AddTrading(trading);

            Context.Current.AddPacket(Player.Client.Connection, new InviteNpcTradeOutgoingPacket(Offers), 

                                                                new JoinNpcTradeOutgoingPacket( (uint)Money, CounterOffers) );

            //TODO: Check if money and items where added, refreshed, removed

            return Promise.Completed;
        }
    }
}