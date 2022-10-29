using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StrongHealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private HashSet<ushort> healthPotions = new HashSet<ushort>() { 7588 };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (healthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                context.AddCommand(new CombatTargetedAttackCommand(command.Player, player, null, MagicEffectType.RedShimmer, (attacker, target) => context.Server.Randomization.Take(200, 400) ) );

                context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Aaaah...") );

                return Promise.FromResult(context);
            }

            return next(context);
        }
    }
}