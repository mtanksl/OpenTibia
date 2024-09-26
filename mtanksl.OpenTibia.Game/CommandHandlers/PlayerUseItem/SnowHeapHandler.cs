using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowHeapHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> snowHeaps;
        private readonly ushort snowBall;

        public SnowHeapHandler()
        {
            snowHeaps = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.snowHeaps") );
            snowBall = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.snowBall") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (snowHeaps.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerCreateItemCommand(command.Player, snowBall, 1) );
            }

            return next();
        }
    }
}