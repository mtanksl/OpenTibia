using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemChestHandler : CommandHandler<PlayerRotateItemCommand>
    {
        private readonly HashSet<ushort> chests;

        public RotateItemChestHandler()
        {
            chests = Context.Server.Values.GetUInt16HashSet("values.items.chests");
        }

        public override Promise Handle(Func<Promise> next, PlayerRotateItemCommand command)
        {
            if (command.Item.UniqueId > 0 && chests.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                return Promise.Break;
            }

            return next();
        }
    }
}