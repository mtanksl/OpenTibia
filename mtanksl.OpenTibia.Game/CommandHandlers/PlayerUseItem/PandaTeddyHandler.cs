using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PandaTeddyHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> pandaTeddies = new HashSet<ushort>() { 5080 };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            if (pandaTeddies.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Hug me!") );

                return Promise.FromResult(context);
            }

            return next(context);
        }
    }
}