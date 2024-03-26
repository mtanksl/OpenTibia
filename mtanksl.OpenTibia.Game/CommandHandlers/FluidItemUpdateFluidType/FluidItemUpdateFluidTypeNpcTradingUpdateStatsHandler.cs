using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class FluidItemUpdateFluidTypeNpcTradingUpdateStatsHandler : CommandHandler<FluidItemUpdateFluidTypeCommand>
    {
        public override Promise Handle(Func<Promise> next, FluidItemUpdateFluidTypeCommand command)
        {
            if (command.FluidItem.Root() is Inventory inventory)
            {
                return next().Then( () =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(inventory.Player);

                        if (trading != null)
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