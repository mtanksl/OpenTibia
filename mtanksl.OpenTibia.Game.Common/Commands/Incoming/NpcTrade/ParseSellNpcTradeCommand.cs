using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseSellNpcTradeCommand : IncomingCommand
    {
        public ParseSellNpcTradeCommand(Player player, SellNpcTradeIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public SellNpcTradeIncomingPacket Packet { get; set; }

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

                    if (offer != null && offer.SellPrice > 0)
                    {                        
                        int count = Math.Max(1, Math.Min(10000, (int)Packet.Count) );

                        PlayerSellNpcTradeEventArgs e = new PlayerSellNpcTradeEventArgs(Player, itemMetadata.OpenTibiaId, Packet.Type, (byte)count, (int)offer.SellPrice * count, Packet.KeepEquipped);

                        Context.AddEvent(trading.OfferNpc, ObserveEventArgs.Create(trading.OfferNpc, e) );
                        
                        Context.AddEvent(Player, e);
                    }
                }
            }

            return Promise.Completed;
        }
    }
}