using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WindowHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> windows;

        public WindowHandler()
        {
            windows = LuaScope.GetInt16Int16Dictionary(Context.Server.Values.GetValue("values.items.transformation.windows") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (windows.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.Parent is HouseTile houseTile && !houseTile.House.CanOpenWindow(command.Player.Name) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                    return Promise.Break;
                }

                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.DoNotDisturb, 100, "Do Not Disturb") ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                } );
            }

            return next();
        }
    }
}