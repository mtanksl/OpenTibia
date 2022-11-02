using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Runes2Handler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private class Rune
        {
            public string Name { get; set; }

            public string Group { get; set; }

            public int GroupCooldownInMilliseconds { get; set; }

            public Func<Context, Player, Creature, bool> Condition { get; set; }

            public Func<Context, Player, Creature, Promise> Callback { get; set; }
        }

        private static HashSet<ushort> itemWithItemRunes = new HashSet<ushort>() { 2285, 2286, 2289, 2301, 2305, 2303, 2277, 2262, 2279, 2302, 2304, 2313, 2293, 2269 };

        private static Dictionary<ushort, Rune> runes = new Dictionary<ushort, Rune>()
        {
            [2265] = new Rune()
            {
                Name = "Intense Healing Rune",

                Group = "Healing",

                GroupCooldownInMilliseconds = 2000,

                Callback = Healing(player => GenericFormula(player.Level, player.Skills.MagicLevel, 70, 30) )
            },

            [2273] = new Rune()
            {
                Name = "Ultimate Healing Rune",

                Group = "Healing",

                GroupCooldownInMilliseconds = 2000,

                Callback = Healing(player => GenericFormula(player.Level, player.Skills.MagicLevel, 250, 0))
            },

            [2287] = new Rune()
            {
                Name = "Light Magic Missile Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Callback = TargetedAttack(ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, player => GenericFormula(player.Level, player.Skills.MagicLevel, 15, 5) )
            },

            [2311] = new Rune()
            {
                Name = "Heavy Magic Missile Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Callback = TargetedAttack(ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, player => GenericFormula(player.Level, player.Skills.MagicLevel, 30, 10) )
            },

            [2268] = new Rune()
            {
                Name = "Sudden Death Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Callback = TargetedAttack(ProjectileType.SuddenDeath, MagicEffectType.MortArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 150, 20) )
            }
        };

        private static Func<Context, Player, Creature, Promise> Healing(Func<Player, (int Min, int Max)> formula)
        {
            return (context, player, target) =>
            {
                var calculated = formula(player);

                return context.AddCommand(new CombatTargetedAttackCommand(player, target, null, MagicEffectType.BlueShimmer, (a, t) => context.Server.Randomization.Take(calculated.Min, calculated.Max) ) );
            };           
        }

        private static Func<Context, Player, Creature, Promise> TargetedAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, Func<Player, (int Min, int Max)> formula)
        {
            return (context, player, target) =>
            {
                var calculated = formula(player);

                return context.AddCommand(new CombatTargetedAttackCommand(player, target, projectileType, magicEffectType, (a, t) => -context.Server.Randomization.Take(calculated.Min, calculated.Max) ) );
            };
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            Rune rune;

            if (runes.TryGetValue(command.Item.Metadata.OpenTibiaId, out rune) )
            {
                CooldownBehaviour behaviour = context.Server.Components.GetComponent<CooldownBehaviour>(command.Player);

                if ( !behaviour.HasCooldown(rune.Group) )
                {
                    if (rune.Condition == null || rune.Condition(context, command.Player, command.ToCreature) )
                    {
                        behaviour.AddCooldown(rune.Group, rune.GroupCooldownInMilliseconds);

                        return Promise.FromResult(context).Then(ctx =>
                        {
                            return rune.Callback(ctx, command.Player, command.ToCreature);

                        } ).Then(ctx =>
                        {
                            return ctx.AddCommand(new ItemDecrementCommand(command.Item, 1) );
                        } );
                    }
                    else
                    {
                        return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                    }
                }
                else
                {
                    return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then(ctx =>
                    {
                        ctx.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );
                    } );
                }                
            }
            else if (itemWithItemRunes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new PlayerUseItemWithItemCommand(command.Player, command.Item, command.ToCreature.Tile.Ground) );
            }

            return next(context);
        }
    }
}