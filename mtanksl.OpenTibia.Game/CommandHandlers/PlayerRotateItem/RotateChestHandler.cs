using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateChestHandler : CommandHandler<PlayerRotateItemCommand>
    {
        private static HashSet<ushort> chests = new HashSet<ushort>() { 1740, 1747, 1748, 1749 };

        public override Promise Handle(Func<Promise> next, PlayerRotateItemCommand command)
        {
            if (command.Item.UniqueId > 0 && chests.Contains(command.Item.Metadata.OpenTibiaId))
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                return Promise.Break;
            }

            return next();
        }
    }
}