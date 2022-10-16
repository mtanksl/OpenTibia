using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Runes2Handler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        Dictionary<ushort, Func<Player, Creature, Action<Context>>> runes = new Dictionary<ushort, Func<Player, Creature, Action<Context>>>()
        {
            { 2265 /* Intense healing */, (player, target) =>
            {
                return Healing(player, target, GenericFormula(player.Level, player.Skills.MagicLevel, 70, 30, 100, 999) );
            } },

            { 2273 /* Ultimate healing */, (player, target) =>
            {
                return Healing(player, target, GenericFormula(player.Level, player.Skills.MagicLevel, 250, 0, 100, 999) );
            } }
        };

        private static Action<Context> Healing(Player player, Creature target, (int Min, int Max) formula)
        {
            return context =>
            {
                context.AddCommand(new CombatTargetedAttackCommand(player, target, null, MagicEffectType.BlueShimmer, _ => Server.Random.Next(formula.Min, formula.Max) ) );
            };           
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation, int min, int max)
        {
            var formula = 3 * magicLevel + 2 * level;

            if (formula < min)
            {
                formula = min;
            }
            else if (formula > max)
            {
                formula = max;
            }

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            Func<Player, Creature, Action<Context>> callback;

            if (runes.TryGetValue(command.Item.Metadata.OpenTibiaId, out callback) )
            {
                return Promise.FromResult(context).Then( callback(command.Player, command.ToCreature) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemDecrementCommand(command.Item, 1) );
                } );
            }

            return next(context);
        }
    }
}