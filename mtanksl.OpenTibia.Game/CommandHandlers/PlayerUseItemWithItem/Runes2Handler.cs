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
    public class Runes2Handler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private class Rune
        {
            public string Name { get; set; }

            public string Group { get; set; }

            public int GroupCooldownInMilliseconds { get; set; }

            public Func<Player, Tile, bool> Condition { get; set; }

            public Func<Player, Tile, Promise> Callback { get; set; }
        }

        private static Dictionary<ushort, Rune> runes = new Dictionary<ushort, Rune>()
        {
            [2285] = new Rune()
            {
                Name = "Poison Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) )  )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Poisoned,

                        MagicEffectType = MagicEffectType.GreenRings,

                        AnimatedTextColor = AnimatedTextColor.Green,

                        Damages = new[] { -5, -5, -5, -5, -5, -4, -4, -4, -4, -4, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2286] = new Rune()
            {
                Name = "Poison Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                        new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Poisoned,

                        MagicEffectType = MagicEffectType.GreenRings,

                        AnimatedTextColor = AnimatedTextColor.Green,

                        Damages = new[] { -5, -5, -5, -5, -5, -4, -4, -4, -4, -4, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2289] = new Rune()
            {
                Name = "Poison Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Poisoned,

                        MagicEffectType = MagicEffectType.GreenRings,

                        AnimatedTextColor = AnimatedTextColor.Green,

                        Damages = new[] { -5, -5, -5, -5, -5, -4, -4, -4, -4, -4, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2301] = new Rune()
            {
                Name = "Fire Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Burning,

                        MagicEffectType = MagicEffectType.FirePlume,

                        AnimatedTextColor = AnimatedTextColor.Orange,

                        Damages = new[] { -20, -10, -10 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2305] = new Rune()
            {
                Name = "Fire Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                        new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Burning,

                        MagicEffectType = MagicEffectType.FirePlume,

                        AnimatedTextColor = AnimatedTextColor.Orange,

                        Damages = new[] { -20, -10, -10 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2303] = new Rune()
            {
                Name = "Fire Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Burning,

                        MagicEffectType = MagicEffectType.FirePlume,

                        AnimatedTextColor = AnimatedTextColor.Orange,

                        Damages = new[] { -20, -10, -10 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2277] = new Rune()
            {
                Name = "Energy Field Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Electrified,

                        MagicEffectType = MagicEffectType.EnergyDamage,

                        AnimatedTextColor = AnimatedTextColor.LightBlue,

                        Damages = new[] { -35, -25, -25 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2262] = new Rune()
            {
                Name = "Energy Bomb Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                        new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Electrified,

                        MagicEffectType = MagicEffectType.EnergyDamage,

                        AnimatedTextColor = AnimatedTextColor.LightBlue,

                        Damages = new[] { -35, -25, -25 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2279] = new Rune()
            {
                Name = "Energy Wall Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, new ConditionDto()
                    {
                        SpecialCondition = SpecialCondition.Electrified,

                        MagicEffectType = MagicEffectType.EnergyDamage,

                        AnimatedTextColor = AnimatedTextColor.LightBlue,

                        Damages = new[] { -35, -25, -25 },

                        IntervalInMilliseconds = 2000
                    } ) );
                }
            },

            [2302] = new Rune()
            {
                Name = "Fireball Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
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

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneAsRadialCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, (attacker, target) => -Context.Current.Server.Randomization.Take(damage.Min, damage.Max) ) );
                }
            },

            [2304] = new Rune()
            {
                Name = "Great Fireball Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
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

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneAsRadialCommand(attacker, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, (attacker, target) => -Context.Current.Server.Randomization.Take(damage.Min, damage.Max) ) );
                }
            },

            [2313] = new Rune()
            {
                Name = "Explosion Rune",

                Group = "Attack",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                                            new Offset(0, -1),
                        new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                                            new Offset(0, 1)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 60, 40);

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneAsRadialCommand(attacker, tile.Position, area, ProjectileType.Explosion, MagicEffectType.ExplosionArea, (attacker, target) => -Context.Current.Server.Randomization.Take(damage.Min, damage.Max) ) );
                }
            },

            [2293] = new Rune()
            {
                Name = "Magic Wall Rune",

                Group = "Support",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, null, 1497, 1, null) );
                }
            },

            [2269] = new Rune()
            {
                Name = "Wild Growth Rune",

                Group = "Support",

                GroupCooldownInMilliseconds = 2000,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker, tile) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 0)
                    };

                    return Context.Current.AddCommand(new CombatAttackAreaWithRuneCreateItemCommand(attacker, tile.Position, area, ProjectileType.Energy, null, 1499, 1, null) );
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
                PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.Components.GetComponent<PlayerCooldownBehaviour>(command.Player);

                if ( !playerCooldownBehaviour.HasCooldown(rune.Group) )
                {
                    if (rune.Condition == null || rune.Condition(command.Player, toTile) )
                    {
                        playerCooldownBehaviour.AddCooldown(rune.Group, rune.GroupCooldownInMilliseconds);

                        return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                        {
                            return rune.Callback(command.Player, toTile);
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