using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyNpcTradingRejectHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                if (Context.Server.NpcTradings.Count > 0)
                {
                    NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(command.Player);

                    if (trading != null)
                    {
                        Context.Server.NpcTradings.RemoveTrading(trading);
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}