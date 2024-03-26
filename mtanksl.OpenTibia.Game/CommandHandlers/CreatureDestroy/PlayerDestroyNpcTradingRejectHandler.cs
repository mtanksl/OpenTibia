using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyNpcTradingRejectHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    if (Context.Server.NpcTradings.Count > 0)
                    {
                        NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(player);

                        if (trading != null)
                        {
                            Context.Server.NpcTradings.RemoveTrading(trading);
                        }
                    }

                    return Promise.Completed;
                } );
            }

            return next();            
        }
    }
}