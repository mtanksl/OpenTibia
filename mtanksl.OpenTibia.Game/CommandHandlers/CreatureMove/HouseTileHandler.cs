using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class HouseTileHandler : CommandHandler<CreatureMoveCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureMoveCommand command)
        {
            if (command.Creature is Player player && command.ToTile is HouseTile houseTile && !houseTile.House.CanWalk(player.Name) )
            {
                Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreNotInvited) );

                Context.AddPacket(player, new StopWalkOutgoingPacket(player.Direction) );

                return Promise.Break;
            }

            return next();
        }
    }
}