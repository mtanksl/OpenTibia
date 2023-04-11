using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PandaTeddyHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> pandaTeddies = new HashSet<ushort>() { 5080 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (pandaTeddies.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Hug me!") );

                return Promise.Completed;
            }

            return next();
        }
    }
}