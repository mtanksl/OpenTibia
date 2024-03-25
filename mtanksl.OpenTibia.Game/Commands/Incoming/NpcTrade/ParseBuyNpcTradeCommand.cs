using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseBuyNpcTradeCommand : Command
    {
        public ParseBuyNpcTradeCommand(Player player, BuyNpcTradeIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public BuyNpcTradeIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GameplayPrivateNpcSystem)
            {
                NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByTibiaId(Packet.TibiaId);
                    
                    OfferDto offer = trading.Offers
                        .Where(o => o.TibiaId == Packet.TibiaId &&
                                    o.Type == Packet.Type)
                        .FirstOrDefault();

                    if (offer != null && offer.BuyPrice > 0)
                    {
                        MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>(trading.OfferNpc);

                        if (npcThinkBehaviour != null)
                        {
                            int count = Math.Max(1, Math.Min(100, (int)Packet.Count) );

                            return npcThinkBehaviour.Buy(Player, itemMetadata.OpenTibiaId, Packet.Type, (byte)count, (int)offer.BuyPrice * count, Packet.IgnoreCapacity, Packet.BuyWithBackpacks);
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}