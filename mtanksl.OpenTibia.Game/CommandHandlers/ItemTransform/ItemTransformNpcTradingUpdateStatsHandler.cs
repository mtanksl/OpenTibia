using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemTransformNpcTradingUpdateStatsHandler : CommandResultHandler<Item, ItemTransformCommand>
    {  
        private readonly ushort goldCoin;
        private readonly ushort platinumCoin;
        private readonly ushort crystalCoin;

        public ItemTransformNpcTradingUpdateStatsHandler()
        {
            goldCoin = Context.Server.Values.GetUInt16("values.items.goldCoin");
            platinumCoin = Context.Server.Values.GetUInt16("values.items.platinumCoin");
            crystalCoin = Context.Server.Values.GetUInt16("values.items.crystalCoin");
        }

        public override PromiseResult<Item> Handle(Func<PromiseResult<Item>> next, ItemTransformCommand command)
        {
            if (command.Item.Root() is Inventory inventory)
            {
                return next().Then( (toItem) =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(inventory.Player);

                        if (trading != null && (command.Item.Metadata.OpenTibiaId == goldCoin || command.Item.Metadata.OpenTibiaId == platinumCoin || command.Item.Metadata.OpenTibiaId == crystalCoin || trading.Offers.Any(o => o.TibiaId == command.Item.Metadata.TibiaId) || toItem.Metadata.OpenTibiaId == goldCoin || toItem.Metadata.OpenTibiaId == platinumCoin || toItem.Metadata.OpenTibiaId == crystalCoin || trading.Offers.Any(o => o.TibiaId == toItem.Metadata.TibiaId) ) )
                        {
                            return Context.AddCommand(new NpcTradeUpdateStatsCommand(trading) ).Then( () =>
                            {
                                return Promise.FromResult(toItem);
                            } );
                        }
                    }

                    return Promise.FromResult(toItem);
                } );
            }

            return next();
        }
    }
}