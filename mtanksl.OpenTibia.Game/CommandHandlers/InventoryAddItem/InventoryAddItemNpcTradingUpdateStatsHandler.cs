using OpenTibia.Game.Commands;
using System;

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

                    if (trading != null)
                    {
                        return Context.AddCommand(new NpcTradeUpdateStatsCommand(trading) );
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}