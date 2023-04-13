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

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) )  )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, (attacker, target) => 0) );
                }
            },

            [2286] = new Rune()
            {
                Name = "Poison Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                        new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, (attacker, target) => 0) );
                }
            },

            [2289] = new Rune()
            {
                Name = "Poison Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, (attacker, target) => 0) );
                }
            },

            [2301] = new Rune()
            {
                Name = "Fire Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, (attacker, target) => 0) );
                }
            },

            [2305] = new Rune()
            {
                Name = "Fire Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                        new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, (attacker, target) => 0) );
                }
            },

            [2303] = new Rune()
            {
                Name = "Fire Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, (attacker, target) => 0) );
                }
            },

            [2277] = new Rune()
            {
                Name = "Energy Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, (attacker, target) => 0) );
                }
            },

            [2262] = new Rune()
            {
                Name = "Energy Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                        new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, (attacker, target) => 0) );
                }
            },

            [2279] = new Rune()
            {
                Name = "Energy Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, (attacker, target) => 0) );
                }
            },

            [2302] = new Rune()
            {
                Name = "Fireball Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                                            new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2),
                        new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1),
                        new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),
                        new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),
                                            new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 20, 5);

                    return context.AddCommand(new CombatAttackAreaWithRuneAsRadialCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
                }
            },

            [2304] = new Rune()
            {
                Name = "Great Fireball Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                                                                new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
                                            new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
                        new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
                        new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
                        new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
                                            new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
                                                                new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 50, 15);

                    return context.AddCommand(new CombatAttackAreaWithRuneAsRadialCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
                }
            },

            [2313] = new Rune()
            {
                Name = "Explosion Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                                            new Offset(0, -1),
                        new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                                            new Offset(0, 1)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 60, 40);

                    return context.AddCommand(new CombatAttackAreaWithRuneAsRadialCommand(attacker, tile.Position, area, ProjectileType.Explosion, MagicEffectType.ExplosionArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
                }
            },

            [2293] = new Rune()
            {
                Name = "Magic Wall Rune",

                Group = "Support",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, null, 1497, 1, (attacker, target) => 0) );
                }
            },

            [2269] = new Rune()
            {
                Name = "Wild Growth Rune",

                Group = "Support",

                GroupCooldownInMilliseconds = 2000,

                Condition = (context, attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return context.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, null, 1499, 1, (attacker, target) => 0) );
                }
            }
        };

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            Rune rune;

            if (runes.TryGetValue(command.Item.Metadata.OpenTibiaId, out rune) && command.ToItem.Parent is Tile toTile)
            {
                CooldownBehaviour component = Context.Server.Components.GetComponent<CooldownBehaviour>(command.Player);

                if ( !component.HasCooldown(rune.Group) )
                {
                    if (rune.Condition == null || rune.Condition(Context, command.Player, toTile) )
                    {
                        component.AddCooldown(rune.Group, rune.GroupCooldownInMilliseconds);

                        return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                        {
                            return rune.Callback(Context, command.Player, toTile);
                        } );
                    }

                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                        return Promise.Break;
                    } );
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );

                    return Promise.Break;
                } );
            }

            return next();
        }
    }
}