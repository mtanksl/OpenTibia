using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
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
            lockedDoors = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.lockedDoors") );
        }

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