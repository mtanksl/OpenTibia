using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LockedDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> lockedDoors = new HashSet<ushort>() { 1209, 1212, 1231, 1234, 1249, 1252, 3535, 3544, 4913, 4916, 5098, 5107, 5116, 5125, 5134, 5137, 5140, 5143, 5278, 5281, 5732, 5735, 6192, 6195, 6249, 6252, 6799, 6801, 6891, 6900, 7033, 7042, 8541, 8544, 9165, 9168, 9267, 9270 };

        //TODO: More items

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (lockedDoors.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "It is locked.") );

                return Promise.Completed;
            }

            return next();
        }
    }
}