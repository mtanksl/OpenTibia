using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseLookItemNpcTradeCommand : IncomingCommand
    {
        public ParseLookItemNpcTradeCommand(Player player, ushort tibiaId, byte type)
        {
            Player = player;

            TibiaId = tibiaId;

            Type = type;
        }

        public Player Player { get; set; }

        public ushort TibiaId { get; set; }

        public byte Type { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GameplayPrivateNpcSystem && Context.Server.Features.HasFeatureFlag(FeatureFlag.NpcsChannel) )
            {
                NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByTibiaId(TibiaId);
                    
                    OfferDto offer = trading.Offers
                        .Where(o => o.TibiaId == TibiaId &&
                                    o.Type == Type)
                        .FirstOrDefault();

                    if (offer != null && (offer.BuyPrice > 0 || offer.SellPrice > 0) )
                    {
                        return Context.AddCommand(new PlayerLookItemCommand(Player, itemMetadata, Type) );                        
                    }
                }
            }

            return Promise.Completed;
        }
    }
}