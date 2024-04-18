using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GarlicBreadOrCookie : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> garlicBreadOrCookie = new HashSet<ushort>() { 9111, 9116 };

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