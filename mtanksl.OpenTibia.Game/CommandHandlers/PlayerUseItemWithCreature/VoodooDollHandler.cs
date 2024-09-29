using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class VoodooDollHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private readonly HashSet<ushort> voodooDolls;

        public VoodooDollHandler()
        {
            voodooDolls = Context.Server.Values.GetUInt16HashSet("values.items.voodooDolls");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (voodooDolls.Contains(command.Item.Metadata.OpenTibiaId) && (command.ToCreature is Player || command.ToCreature is Npc) )
            {
                if (Context.Server.Randomization.HasProbability(1.0 / 5) )
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.ToCreature, MagicEffectType.RedSpark) ).Then( () =>
                    {
                        return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "You concentrate on your victim and hit the needle in the doll.") );
                    } );
                }

                return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "You concentrate on your victim, hit the needle in the doll.......but nothing happens.") );
            }

            return next();
        }
    }
}