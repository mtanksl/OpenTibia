using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System;
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
                Context.Server.NpcTradings.RemoveTrading(trading); //TODO: Destroy
            }

            trading = new NpcTrading()
            {
                OfferNpc = Npc,

                CounterOfferPlayer = Player,

                Offers = Offers
            };

            Context.Server.NpcTradings.AddTrading(trading);


            int money = SumMoney(Player.Inventory);

            List<CounterOfferDto> counterOffers = new List<CounterOfferDto>();

            foreach (var offer in Offers)
            {
                if (offer.SellPrice > 0)
                {
                    int count = CountItems(Player.Inventory, offer.TibiaId, offer.Count);

                    if (count > 0)
                    {
                        counterOffers.Add(new CounterOfferDto(offer.TibiaId, (byte)Math.Max(count, 100) ) );
                    }
                }
            }

            Context.Current.AddPacket(Player.Client.Connection, new InviteNpcTradeOutgoingPacket(Offers), 

                                                                new JoinNpcTradeOutgoingPacket( (uint)money, counterOffers) );

            //TODO: Check if money and items where added, refreshed, removed

            return Promise.Completed;
        }

        private int SumMoney(IContainer parent)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents())
            {
                if (content is Container container)
                {
                    sum += SumMoney(container);
                }

                if (content.Metadata.OpenTibiaId == 2160) // Crystal coin
                {
                    sum += ( (StackableItem)content).Count * 10000;
                }
                else if (content.Metadata.OpenTibiaId == 2152) // Platinum coin
                {
                    sum += ( (StackableItem)content).Count * 100;
                }
                else if (content.Metadata.OpenTibiaId == 2148) // Gold coin
                {
                    sum += ( (StackableItem)content).Count * 1;
                }
            }

            return sum;
        }

        private int CountItems(IContainer parent, ushort tibiaId, byte type)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += CountItems(container, tibiaId, type);
                }

                if (content.Metadata.TibiaId == tibiaId)
                {
                    if (content is StackableItem stackableItem)
                    {
                        sum += stackableItem.Count;
                    }
                    else if (content is FluidItem fluidItem)
                    {
                        if (fluidItem.FluidType == (FluidType)type)
                        {
                            sum += 1;
                        }
                    }
                    else
                    {
                        sum += 1;
                    }
                }
            }

            return sum;
        }
    }
}