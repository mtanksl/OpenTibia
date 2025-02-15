using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemDestroyNpcTradingUpdateStatsHandler : CommandHandler<ItemDestroyCommand>
    {  
        private readonly ushort goldCoin;
        private readonly ushort platinumCoin;
        private readonly ushort crystalCoin;

        public ItemDestroyNpcTradingUpdateStatsHandler()
        {
            goldCoin = Context.Server.Values.GetUInt16("values.items.goldCoin");
            platinumCoin = Context.Server.Values.GetUInt16("values.items.platinumCoin");
            crystalCoin = Context.Server.Values.GetUInt16("values.items.crystalCoin");
        }

        public override Promise Handle(Func<Promise> next, ItemDestroyCommand command)
        {
            if (command.Item.Root() is Inventory inventory)
            {
                return next().Then( () =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(inventory.Player);

                        if (trading != null && (command.Item.Metadata.OpenTibiaId == goldCoin || command.Item.Metadata.OpenTibiaId == platinumCoin || command.Item.Metadata.OpenTibiaId == crystalCoin || trading.Offers.Any(o => o.TibiaId == command.Item.Metadata.TibiaId) ) )
                        {
                            return Context.AddCommand(new NpcTradeUpdateStatsCommand(trading) );
                        }
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}