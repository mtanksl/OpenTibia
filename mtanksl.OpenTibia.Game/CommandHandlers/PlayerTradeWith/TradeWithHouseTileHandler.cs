using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TradeWithHouseTileHandler : CommandHandler<PlayerTradeWithCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerTradeWithCommand command)
        {
            HouseTile fromHouseTile = null;

            if (command.Item.Parent is HouseTile p1)
            {
                fromHouseTile = p1;
            }
            else if (command.Item.Parent is Container fromContainer && fromContainer.Root() is HouseTile p2)
            {
                fromHouseTile = p2;
            }

            if (fromHouseTile != null && !fromHouseTile.House.CanWalk(command.Player.Name) )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                return Promise.Break;
            }

            return next();
        }
    }
}