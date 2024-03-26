using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class NpcTradeUpdateStatsCommand : Command
    {
        public NpcTradeUpdateStatsCommand(NpcTrading trading)
        {
            Trading = trading;
        }

        public NpcTrading Trading { get; set; }

        public override Promise Execute()
        {
            int money = SumMoney(Trading.CounterOfferPlayer.Inventory);

            List<CounterOfferDto> counterOffers = new List<CounterOfferDto>();

            foreach (var offer in Trading.Offers)
            {
                if (offer.SellPrice > 0)
                {
                    int count = CountItems(Trading.CounterOfferPlayer.Inventory, offer.TibiaId, offer.Type);

                    if (count > 0)
                    {
                        counterOffers.Add(new CounterOfferDto(offer.TibiaId, (byte)Math.Min(count, 100) ) );
                    }
                }
            }

            Context.AddPacket(Trading.CounterOfferPlayer, new JoinNpcTradeOutgoingPacket( (uint)money, counterOffers) );

            return Promise.Completed;
        }

        private static int SumMoney(IContainer parent)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
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

        private static int CountItems(IContainer parent, ushort tibiaId, byte type)
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