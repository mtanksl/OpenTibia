using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
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
            if (Context.Server.Config.GamePrivateNpcSystem)
            {
                NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByTibiaId(Packet.TibiaId);
                    
                    OfferDto offer = trading.Offers
                        .Where(o => o.TibiaId == Packet.TibiaId)
                        .FirstOrDefault();

                    if (offer != null && offer.BuyPrice > 0)
                    {
                        MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>(trading.OfferNpc);

                        if (npcThinkBehaviour != null)
                        {
                            return npcThinkBehaviour.Buy(Player, itemMetadata.OpenTibiaId, Packet.Type, Packet.Count, (int)offer.BuyPrice * Packet.Count, Packet.IgnoreCapacity, Packet.BuyWithBackpacks).Then( () =>
                            {
                                return Promise.FromResultAsEmptyObjectArray;
                            } );
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}