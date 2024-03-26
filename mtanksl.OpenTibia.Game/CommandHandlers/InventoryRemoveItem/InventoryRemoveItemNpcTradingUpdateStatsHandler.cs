using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryRemoveItemNpcTradingUpdateStatsHandler : CommandHandler<InventoryRemoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, InventoryRemoveItemCommand command)
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