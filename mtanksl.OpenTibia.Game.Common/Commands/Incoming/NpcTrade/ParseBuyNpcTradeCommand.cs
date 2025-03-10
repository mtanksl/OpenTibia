using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseBuyNpcTradeCommand : IncomingCommand
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
            if (Context.Server.Config.GameplayPrivateNpcSystem && Context.Server.Features.HasFeatureFlag(FeatureFlag.NpcsChannel) )
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
                        int count = Math.Max(1, Math.Min(100, (int)Packet.Count) );

                        PlayerBuyNpcTradeEventArgs e = new PlayerBuyNpcTradeEventArgs(Player, itemMetadata.OpenTibiaId, Packet.Type, (byte)count, (int)offer.BuyPrice * count, Packet.IgnoreCapacity, Packet.BuyWithBackpacks);

                        Context.AddEvent(trading.OfferNpc, ObserveEventArgs.Create(trading.OfferNpc, e) );
                        
                        Context.AddEvent(Player, e);
                    }
                }
            }

            return Promise.Completed;
        }
    }
}