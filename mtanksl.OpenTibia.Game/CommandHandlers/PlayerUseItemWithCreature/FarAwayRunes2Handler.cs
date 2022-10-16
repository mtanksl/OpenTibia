using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FarAwayRunes2Handler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private ushort intenseHealingRune = 2265;

        private ushort ultimateHealingRune = 2273;

        private ushort suddenDeath = 2268;

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            Action<Context> callback = null;

            if (command.Item.Metadata.OpenTibiaId == intenseHealingRune)
            {
                var formula = Generic(command.Player.Level, command.Player.Skills.MagicLevel, 70, 30, 100, 999);

                callback = Healing(command.Player, command.Item, formula.Min, formula.Max);
            }
            else if (command.Item.Metadata.OpenTibiaId == ultimateHealingRune)
            {
                var formula = Generic(command.Player.Level, command.Player.Skills.MagicLevel, 250, 0, 100, 999);

                callback = Healing(command.Player, command.Item, formula.Min, formula.Max);
            }
            else if (command.Item.Metadata.OpenTibiaId == suddenDeath)
            {
                var formula = Generic(command.Player.Level, command.Player.Skills.MagicLevel, 150, 20);

                callback = TargetedAttack(command.Player, command.Item, command.ToCreature, ProjectileType.Death, MagicEffectType.MortArea, formula.Min, formula.Max);
            }

            if (callback != null)
            {
                return Promise.FromResult(context).Then(callback);
            }

            return next(context);
        }

        private Action<Context> Healing(Player player, Item item, int min, int max)
        {
            return context =>
            {
                context.AddCommand(new CombatTargetedAttackCommand(player, player, null, MagicEffectType.BlueShimmer, target => Server.Random.Next(min, max) ) );

                context.AddCommand(new ItemDecrementCommand(item, 1) );
            };           
        }

        private Action<Context> TargetedAttack(Player player, Item item, Creature target, ProjectileType projectileType, MagicEffectType? magicEffectType, int min, int max)
        {
            return context =>
            {
                context.AddCommand(new CombatTargetedAttackCommand(player, target, projectileType, magicEffectType, _ => -Server.Random.Next(min, max) ) );

                context.AddCommand(new ItemDecrementCommand(item, 1) );
            };
        }

        private (int Min, int Max) Generic(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        private (int Min, int Max) Generic(int level, int magicLevel, int @base, int variation, int min, int max)
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
    }
}