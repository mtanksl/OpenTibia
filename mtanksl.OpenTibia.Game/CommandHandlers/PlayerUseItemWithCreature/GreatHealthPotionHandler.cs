using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GreatHealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private HashSet<ushort> healthPotions = new HashSet<ushort>() { 7591 };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (healthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                context.AddCommand(CombatCommand.TargetAttack(command.Player, player, null, MagicEffectType.RedShimmer, (attacker, target) => context.Server.Randomization.Take(500, 700) ) );

                context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Aaaah...") );

                return Promise.FromResult(context);
            }

            return next(context);
        }
    }
}