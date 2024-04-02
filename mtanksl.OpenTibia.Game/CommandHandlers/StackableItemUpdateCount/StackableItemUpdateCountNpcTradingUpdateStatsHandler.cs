using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class StackableItemUpdateCountNpcTradingUpdateStatsHandler : CommandHandler<StackableItemUpdateCountCommand>
    {
        public override Promise Handle(Func<Promise> next, StackableItemUpdateCountCommand command)
        {
            if (command.StackableItem.Root() is Inventory inventory)
            {
                return next().Then( () =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(inventory.Player);

                        if (trading != null && (command.StackableItem.Metadata.OpenTibiaId == 2148 || command.StackableItem.Metadata.OpenTibiaId == 2152 || command.StackableItem.Metadata.OpenTibiaId == 2160 || trading.Offers.Any(o => o.TibiaId == command.StackableItem.Metadata.TibiaId) ) )
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