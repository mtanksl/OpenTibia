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

            public TimeSpan GroupCooldown { get; set; }

            public int Level { get; set; }

            public int MagicLevel { get; set; }

            public Func<Player, Tile, bool> Condition { get; set; }

            public Func<Player, Tile, Promise> Callback { get; set; }
        }

        private static Dictionary<ushort, Rune> runes = new Dictionary<ushort, Rune>()
        {
            [2285] = new Rune()
            {
                Name = "Poison Field Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 14,

                MagicLevel = 0,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) )  )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 5, 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2286] = new Rune()
            {
                Name = "Poison Bomb Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 25,

                MagicLevel = 4,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 5, 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2289] = new Rune()
            {
                Name = "Poison Wall Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 29,

                MagicLevel = 5,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 5, 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2301] = new Rune()
            {
                Name = "Fire Field Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 15,

                MagicLevel = 1,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Burning, MagicEffectType.FirePlume, AnimatedTextColor.Orange, new[] { 20, 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2305] = new Rune()
            {
                Name = "Fire Bomb Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 27,

                MagicLevel = 5,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Burning, MagicEffectType.FirePlume, AnimatedTextColor.Orange, new[] { 20, 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2303] = new Rune()
            {
                Name = "Fire Wall Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 33,

                MagicLevel = 6,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Burning, MagicEffectType.FirePlume, AnimatedTextColor.Orange, new[] { 20, 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2277] = new Rune()
            {
                Name = "Energy Field Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 18,

                MagicLevel = 3,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Electrified, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, new[] { 30, 25, 25 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2262] = new Rune()
            {
                Name = "Energy Bomb Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 37,

                MagicLevel = 10,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Electrified, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, new[] { 30, 25, 25 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2279] = new Rune()
            {
                Name = "Energy Wall Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 41,

                MagicLevel = 9,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, 
                        
                                                      new DamageCondition(SpecialCondition.Electrified, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, new[] { 30, 25, 25 }, TimeSpan.FromSeconds(2) ) ) );
                }
            },

            [2302] = new Rune()
            {
                Name = "Fireball Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 27,

                MagicLevel = 4,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 20, 5);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
                }
            },

            [2304] = new Rune()
            {
                Name = "Great Fireball Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 30,

                MagicLevel = 4,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 50, 15);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
                }
            },

            [2313] = new Rune()
            {
                Name = "Explosion Rune",

                Group = "Attack",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 31,

                MagicLevel = 6,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 60, 40);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Explosion, MagicEffectType.ExplosionArea, new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
            },

            [2293] = new Rune()
            {
                Name = "Magic Wall Rune",

                Group = "Support",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 27,

                MagicLevel = 9,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Energy, null, 1497, 1) );
                }
            },

            [2269] = new Rune()
            {
                Name = "Wild Growth Rune",

                Group = "Support",

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 27,

                MagicLevel = 8,

                Condition = (attacker, tile) =>
                {
                    if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, tile.Position, area, ProjectileType.Energy, null, 1499, 1) );
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
                if (command.Player.Level >= rune.Level)
                {
                    if (command.Player.Skills.MagicLevel >= rune.MagicLevel)
                    {
                        PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(command.Player);

                        if ( !playerCooldownBehaviour.HasCooldown(rune.Group) )
                        {
                            if (rune.Condition == null || rune.Condition(command.Player, toTile) )
                            {
                                playerCooldownBehaviour.AddCooldown(rune.Group, rune.GroupCooldown);

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

                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMagicLevel) );
                         
                        return Promise.Break;
                    } );
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughLevel) );
                         
                    return Promise.Break;
                } );
            }

            return next();
        }
    }
}