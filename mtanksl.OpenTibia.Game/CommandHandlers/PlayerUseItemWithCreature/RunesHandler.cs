using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RunesHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private static HashSet<ushort> itemWithItemRunes = new HashSet<ushort>() { 2285, 2286, 2289, 2301, 2305, 2303, 2277, 2262, 2279, 2302, 2304, 2313, 2293, 2269 };

        private static Dictionary<ushort, Rune> runes = new Dictionary<ushort, Rune>()
        {
            [2266] = new Rune()
            {
                Name = "Cure Poison Rune",

                Group = "Healing",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 15,

                MagicLevel = 0,

                Callback = (attacker, target, tile, item) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Poisoned) );
                    } );
                }
            },

            [2265] = new Rune()
            {
                Name = "Intense Healing Rune",

                Group = "Healing",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 15,

                MagicLevel = 1,

                Callback = (attacker, target, tile, item) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 70, 30);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,

                        new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
                }
            },

            [2273] = new Rune()
            {
                Name = "Ultimate Healing Rune",

                Group = "Healing",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 24,

                MagicLevel = 4,

                Callback = (attacker, target, tile, item) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 250, 0);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,

                        new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
                }
            },

            [2287] = new Rune()
            {
                Name = "Light Magic Missile Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 15,

                MagicLevel = 0,

                Callback = (attacker, target, tile, item) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 15, 5);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,

                        new SimpleAttack(ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
                }
            },

            [2311] = new Rune()
            {
                Name = "Heavy Magic Missile Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 25,

                MagicLevel = 3,

                Callback = (attacker, target, tile, item) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 30, 10);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,

                        new SimpleAttack(ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
                }
            },

            [2268] = new Rune()
            {
                Name = "Sudden Death Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 45,

                MagicLevel = 15,

                Callback = (attacker, target, tile, item) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 150, 20);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,

                        new SimpleAttack(ProjectileType.SuddenDeath, MagicEffectType.MortArea, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
            }
        };

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override async Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            Rune rune;

            if ( !runes.TryGetValue(command.Item.Metadata.OpenTibiaId, out rune) )
            {
                RunePlugin plugin = Context.Server.Plugins.GetRunePlugin(true, command.Item.Metadata.OpenTibiaId);

                if (plugin != null)
                {
                    rune = plugin.Rune;
                }
            }

            if (rune != null)
            {
                if (command.Player.Level < rune.Level)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughLevel) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (command.Player.Skills.MagicLevel < rune.MagicLevel)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMagicLevel) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (command.Player.Tile.ProtectionZone)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackAPersonWhileYouAreInAProtectionZone) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (command.ToCreature.Tile.ProtectionZone)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackAPersonInAProtectionZone) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(command.Player);

                if (playerCooldownBehaviour.HasCooldown(rune.Group) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (rune.Condition != null && !await rune.Condition(command.Player, command.ToCreature, null, command.Item) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                playerCooldownBehaviour.AddCooldown(rune.Group, rune.GroupCooldown);

                await Context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                await rune.Callback(command.Player, command.ToCreature, null, command.Item);

                return;
            }
            else if (itemWithItemRunes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                await Context.AddCommand(new PlayerUseItemWithItemCommand(command.Player, command.Item, command.ToCreature.Tile.Ground) );

                return;
            }

            await next();
        }
    }
}