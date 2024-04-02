using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class SplashItemUpdateFluidTypeNpcTradingUpdateStatsHandler : CommandHandler<SplashItemUpdateFluidTypeCommand>
    {
        public override Promise Handle(Func<Promise> next, SplashItemUpdateFluidTypeCommand command)
        {
            if (command.SplashItem.Root() is Inventory inventory)
            {
                return next().Then( () =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(inventory.Player);

                        if (trading != null && trading.Offers.Any(o => o.TibiaId == command.SplashItem.Metadata.TibiaId) )
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