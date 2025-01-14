using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveItemHouseTileHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
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
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

                return Promise.Break;
            }

            HouseTile toHouseTile = null;

            if (command.ToContainer is HouseTile p3)
            {
                toHouseTile = p3;
            }
            else if (command.ToContainer is Container toContainer && toContainer.Root() is HouseTile p4)
            {
                toHouseTile = p4;
            }

            if (toHouseTile != null && !toHouseTile.House.CanWalk(command.Player.Name) )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                return Promise.Break;
            }

            return next();
        }
    }
}