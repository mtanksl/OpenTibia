using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Runes2Handler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private static HashSet<ushort> itemWithItemRunes = new HashSet<ushort>() { 2285, 2286, 2289, 2301, 2305, 2303, 2277, 2262, 2279, 2302, 2304, 2313, 2293, 2269 };

        private static Dictionary<ushort, Func<Player, Creature, Func<Context, Promise>>> runes = new Dictionary<ushort, Func<Player, Creature, Func<Context, Promise>>>()
        {
            { 2265 /* Intense healing */, (player, target) =>
            {
                return Healing(player, target, GenericFormula(player.Level, player.Skills.MagicLevel, 70, 30) );
            } },

            { 2273 /* Ultimate healing */, (player, target) =>
            {
                return Healing(player, target, GenericFormula(player.Level, player.Skills.MagicLevel, 250, 0) );
            } },

            { 2287 /* Light magic missile */, (player, target) =>
            {
                return TargetedAttack(player, target, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, GenericFormula(player.Level, player.Skills.MagicLevel, 15, 5) );
            } },

            { 2311 /* Heavy magic missile */, (player, target) =>
            {
                return TargetedAttack(player, target, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, GenericFormula(player.Level, player.Skills.MagicLevel, 30, 10) );
            } },

            { 2268 /* Sudden death */, (player, target) =>
            {
                return TargetedAttack(player, target, ProjectileType.SuddenDeath, MagicEffectType.MortArea, GenericFormula(player.Level, player.Skills.MagicLevel, 150, 20) );
            } }
        };

        private static Func<Context, Promise> Healing(Player player, Creature target, (int Min, int Max) formula)
        {
            return context =>
            {
                return context.AddCommand(new CombatTargetedAttackCommand(player, target, null, MagicEffectType.BlueShimmer, (a, t) => Server.Random.Next(formula.Min, formula.Max) ) );
            };           
        }

        private static Func<Context, Promise> TargetedAttack(Player player, Creature target, ProjectileType? projectileType, MagicEffectType? magicEffectType, (int Min, int Max) formula)
        {
            return context =>
            {
                return context.AddCommand(new CombatTargetedAttackCommand(player, target, projectileType, magicEffectType, (a, t) => -Server.Random.Next(formula.Min, formula.Max) ) );
            };
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            Func<Player, Creature, Func<Context, Promise>> callback;

            if (runes.TryGetValue(command.Item.Metadata.OpenTibiaId, out callback) )
            {
                return Promise.FromResult(context).Then( callback(command.Player, command.ToCreature) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemDecrementCommand(command.Item, 1) );
                } );
            }
            else if (itemWithItemRunes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new PlayerUseItemWithItemCommand(command.Player, command.Item, command.ToCreature.Tile.Ground) );
            }

            return next(context);
        }
    }
}