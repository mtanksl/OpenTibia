using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TradeWithChestHandler : CommandHandler<PlayerTradeWithCommand>
    {
        private readonly HashSet<ushort> chests;

        public TradeWithChestHandler()
        {
            chests = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.chests") );
        }

        public override Promise Handle(Func<Promise> next, PlayerTradeWithCommand command)
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