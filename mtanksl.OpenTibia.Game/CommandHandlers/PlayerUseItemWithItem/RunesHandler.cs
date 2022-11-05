using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class RunesHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private class Rune
        {
            public string Name { get; set; }

            public string Group { get; set; }

            public int GroupCooldownInMilliseconds { get; set; }

            public Func<Context, Player, Tile, bool> Condition { get; set; }

            public Func<Context, Player, Tile, Promise> Callback { get; set; }
        }

        private static Dictionary<ushort, Rune> runes = new Dictionary<ushort, Rune>()
        {
            [2285] = new Rune()
            {
                Name = "Poison Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) )  )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(0, 0)

                }, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, -5)
            },

            [2286] = new Rune()
            {
                Name = "Poison Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                    new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)

                }, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, -5)
            },

            [2289] = new Rune()
            {
                Name = "Poison Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)

                }, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, -5)
            },

            [2301] = new Rune()
            {
                Name = "Fire Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(0, 0)

                }, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, -20)
            },

            [2305] = new Rune()
            {
                Name = "Fire Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                    new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)

                }, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, -20)
            },

            [2303] = new Rune()
            {
                Name = "Fire Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)

                }, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, -20)
            },

            [2277] = new Rune()
            {
                Name = "Energy Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(0, 0)

                }, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, 1495, 1, -30)
            },

            [2262] = new Rune()
            {
                Name = "Energy Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                    new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)

                }, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, 1495, 1, -30)
            },

            [2279] = new Rune()
            {
                Name = "Energy Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)

                }, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, 1495, 1, -30)
            },

            [2302] = new Rune()
            {
                Name = "Fireball Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaAttack(new Offset[]
                {
                                        new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2),
                    new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1),
                    new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),
                    new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),
                                        new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2)

                }, ProjectileType.Fire, MagicEffectType.FireArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 20, 5) )
            },

            [2304] = new Rune()
            {
                Name = "Great Fireball Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaAttack(new Offset[]
                {
                                                            new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
                                        new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
                    new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
                    new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
                    new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
                                        new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
                                                            new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3)

                }, ProjectileType.Fire, MagicEffectType.FireArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 50, 15) )
            },

            [2313] = new Rune()
            {
                Name = "Explosion Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaAttack(new Offset[]
                {
                                        new Offset(0, -1),
                    new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                                        new Offset(0, 1)

                }, ProjectileType.Explosion, MagicEffectType.ExplosionArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 60, 40) )
            },

            [2293] = new Rune()
            {
                Name = "Magic Wall Rune",

                Group = "Support",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(0, 0)

                }, ProjectileType.Energy, null, 1497, 1, 0)
            },

            [2269] = new Rune()
            {
                Name = "Wild Growth Rune",

                Group = "Support",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, player, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = AreaCreate(new Offset[]
                {
                    new Offset(0, 0)

                }, ProjectileType.Energy, null, 1499, 1, 0)
            }
        };

        private static Func<Context, Player, Tile, Promise> AreaAttack(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Func<Player, (int Min, int Max)> formula)
        {
            return (context, player, tile) =>
            {
                var calculated = formula(player);

                return context.AddCommand(CombatCommand.AreaAttack(player, tile.Position, area, projectileType, magicEffectType, (attacker, target) => -context.Server.Randomization.Take(calculated.Min, calculated.Max) ) );
            };
        }

        private static Func<Context, Player, Tile, Promise> AreaCreate(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, int health)
        {
            return (context, player, tile) =>
            {
                return context.AddCommand(CombatCommand.AreaCreate(player, tile.Position, area, projectileType, magicEffectType, openTibiaId, count, (attacker, target) => health) );
            };
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithItemCommand command)
        {
            Rune rune;

            if (runes.TryGetValue(command.Item.Metadata.OpenTibiaId, out rune) && command.ToItem.Parent is Tile toTile)
            {
                CooldownBehaviour behaviour = context.Server.Components.GetComponent<CooldownBehaviour>(command.Player);

                if ( !behaviour.HasCooldown(rune.Group) )
                {
                    if (rune.Condition == null || rune.Condition(context, command.Player, toTile) )
                    {
                        behaviour.AddCooldown(rune.Group, rune.GroupCooldownInMilliseconds);

                        return Promise.FromResult(context).Then(ctx =>
                        {
                            return rune.Callback(ctx, command.Player, toTile);

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

            return next(context);
        }
    }
}