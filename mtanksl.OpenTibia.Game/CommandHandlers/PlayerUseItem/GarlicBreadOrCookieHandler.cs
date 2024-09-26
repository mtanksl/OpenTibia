using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GarlicBreadOrCookieHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> garlicBreadOrCookie;

        public GarlicBreadOrCookieHandler()
        {
            garlicBreadOrCookie = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.garlicBreadOrCookie") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (garlicBreadOrCookie.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "After taking a small bite you decide that you don't want to eat that.") );
            }

            return next();
        }
    }
}