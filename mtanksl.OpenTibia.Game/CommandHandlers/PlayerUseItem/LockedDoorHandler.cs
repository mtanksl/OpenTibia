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
        private readonly HashSet<ushort> lockedDoors;

        public LockedDoorHandler()
        {
            lockedDoors = Context.Server.Values.GetUInt16HashSet("values.items.lockedDoors");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (lockedDoors.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "It is locked.") );

                return Promise.Completed;
            }

            return next();
        }
    }
}