using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryAddItemNpcTradingUpdateStatsHandler : CommandHandler<InventoryAddItemCommand>
    {
        public override Promise Handle(Func<Promise> next, InventoryAddItemCommand command)
        {
            return next().Then( () =>
            {
                if (Context.Server.NpcTradings.Count > 0)
                {
                    NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(command.Inventory.Player);

                    if (trading != null && (command.Item.Metadata.OpenTibiaId == 2148 || command.Item.Metadata.OpenTibiaId == 2152 || command.Item.Metadata.OpenTibiaId == 2160 || trading.Offers.Any(o => o.TibiaId == command.Item.Metadata.TibiaId)))
                    {
                        return Context.AddCommand(new NpcTradeUpdateStatsCommand(trading));
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}