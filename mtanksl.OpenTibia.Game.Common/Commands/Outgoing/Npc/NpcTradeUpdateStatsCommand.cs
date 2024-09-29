using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class NpcTradeUpdateStatsCommand : Command
    {
        private readonly ushort goldCoin;
        private readonly ushort platinumCoin;
        private readonly ushort crystalCoin;

        public NpcTradeUpdateStatsCommand(NpcTrading trading)
        {
            Trading = trading;

            goldCoin = Context.Server.Values.GetUInt16("values.items.goldCoin");
            platinumCoin = Context.Server.Values.GetUInt16("values.items.platinumCoin");
            crystalCoin = Context.Server.Values.GetUInt16("values.items.crystalCoin");
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

        private int SumMoney(IContainer parent)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += SumMoney(container);
                }

                if (content.Metadata.OpenTibiaId == crystalCoin)
                {
                    sum += ( (StackableItem)content).Count * 10000;
                }
                else if (content.Metadata.OpenTibiaId == platinumCoin)
                {
                    sum += ( (StackableItem)content).Count * 100;
                }
                else if (content.Metadata.OpenTibiaId == goldCoin)
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