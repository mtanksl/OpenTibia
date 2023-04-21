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
    public class SpellsHandler : CommandHandler<PlayerSayCommand>
    {
        private class Spell
        {
            public string Name { get; set; }

            public string Group { get; set; }

            public int CooldownInMilliseconds { get; set; }

            public int GroupCooldownInMilliseconds { get; set; }

            public bool Premium { get; set; }

            public int Mana { get; set; }

            public Func<Player, bool> Condition { get; set; }

            public Func<Player, Promise> Callback { get; set; }
        }

        private static HashSet<ushort> ropeSpots = new HashSet<ushort> { 384, 418 };

        private Dictionary<string, Spell> spells = new Dictionary<string, Spell>()
        {
            ["exani tera"] = new Spell()
            {
                Name = "Magic Rope",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 20,

                Condition = (attacker) =>
                {
                    if (ropeSpots.Contains(attacker.Tile.Ground.Metadata.OpenTibiaId) )
                    {
                        return true;
                    }

                    return false;
                },

                Callback = (attacker) =>
                {
                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 1, -1) );

                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.Teleport) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                    } ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureUpdateTileCommand(attacker, toTile, Direction.South) );
                    } );
                }
            },

            ["exani hur up"] = new Spell()
            {
                Name = "Levitate",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 50,

                Condition = (attacker) =>
                {
                    Tile up = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1) );

                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1).Offset(attacker.Direction) );

                    if (up != null || toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker) =>
                {
                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1).Offset(attacker.Direction) );

                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.Teleport) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                    } ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureUpdateTileCommand(attacker, toTile) );
                    } );
                }
            },

            ["exani hur down"] = new Spell()
            {
                Name = "Levitate",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 50,

                Condition = (attacker) =>
                {
                    Tile next = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(attacker.Direction) );

                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, 1).Offset(attacker.Direction) );

                    if (next != null || toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (attacker) =>
                {
                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, 1).Offset(attacker.Direction) );

                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.Teleport) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                    } ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureUpdateTileCommand(attacker, toTile) );
                    } );
                }
            },

            ["utevo lux"] = new Spell()
            {
                Name = "Light",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 20,

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, new ConditionLight(new Light(6, 215), (6 * 60 + 10) * 1000) ) );
                    } );
                }
            },

            ["utevo gran lux"] = new Spell()
            {
                Name = "Great Light",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 60,

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, new ConditionLight(new Light(8, 215), (11 * 60 + 35) * 1000) ) );
                    } );
                }
            },

            ["utevo vis lux"] = new Spell()
            {
                Name = "Ultimate Light",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 140,

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, new ConditionLight(new Light(9, 215), (33 * 60 + 10) * 1000) ) );
                    } );
                }
            },

            ["utana vid"] = new Spell()
            {
                Name = "Invisible",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 440,

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, new ConditionInvisible( (3 * 60 + 20) * 1000) ) );
                    } );
                }
            },

            ["utani hur"] = new Spell()
            {
                Name = "Haste",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 60,

                Callback = (attacker) =>
                {
                    var speed = HasteFormula(attacker.BaseSpeed);

                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, new ConditionHaste(speed, 33 * 1000) ) );
                    } );
                }
            },

            ["utani gran hur"] = new Spell()
            {
                Name = "Strong Haste",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 100,

                Callback = (attacker) =>
                {
                    var speed = StrongHasteFormula(attacker.BaseSpeed);

                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, new ConditionHaste(speed, 22 * 1000) ) );
                    } );
                }
            },
       
            ["utamo vita"] = new Spell()
            {
                Name = "Magic Shield",

                Group = "Support",

                CooldownInMilliseconds = 14000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 50,

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, new ConditionMagicShield(3 * 60 * 1000) ) );
                    } );
                }
            },

            ["exana pox"] = new Spell()
            {
                Name = "Cure Poison",

                Group = "Healing",

                CooldownInMilliseconds = 6000,

                GroupCooldownInMilliseconds = 1000,

                Premium = false,

                Mana = 30,

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureRemoveConditionCommand(attacker, ConditionSpecialCondition.Poisoned) );
                    } );
                }
            },

            ["exura"] = new Spell()
            {
                Name = "Light Healing",

                Group = "Healing",

                CooldownInMilliseconds = 1000,

                GroupCooldownInMilliseconds = 1000,

                Premium = false,

                Mana = 20,

                Callback = (attacker) =>
                {
                    var formula = LightHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    var damage = Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, new HealingAttack(MagicEffectType.BlueShimmer, damage) ) );
                }
            },

            ["exura gran"] = new Spell()
            {
                Name = "Intense Healing",

                Group = "Healing",

                CooldownInMilliseconds = 1000,

                GroupCooldownInMilliseconds = 1000,

                Premium = false,

                Mana = 70,

                Callback = (attacker) =>
                {
                    var formula = IntenseHealingFormula(attacker.Level, attacker.Skills.MagicLevel);
                    
                    var damage = Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, new HealingAttack(MagicEffectType.BlueShimmer, damage) ) );
                }
            },

            ["exura vita"] = new Spell()
            {
                Name = "Ultimate Healing",

                Group = "Healing",

                CooldownInMilliseconds = 1000,

                GroupCooldownInMilliseconds = 1000,

                Premium = false,

                Mana = 160,

                Callback = (attacker) =>
                {
                    var formula = UltimateHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    var damage = Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, new HealingAttack(MagicEffectType.BlueShimmer, damage) ) );
                }
            },

            ["exura gran mas res"] = new Spell()
            {
                Name = "Mass Healing",

                Group = "Healing",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 1000,

                Premium = true,

                Mana = 150,

                Callback = (attacker) =>
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

                    var formula = MassHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    var damage = Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlueShimmer, new HealingAttack(null, damage) ) );
                }
            },

            ["exori mort"] = new Spell()
            {
                Name = "Death Strike",

                Group = "Attack",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 20,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.MortArea, new SimpleAttack(null, null, AnimatedTextColor.DarkRed, damage) ) );
                }
            },

            ["exori flam"] = new Spell()
            {
                Name = "Flame Strike",

                Group = "Attack",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 20,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.FirePlume, new SimpleAttack(null, null, AnimatedTextColor.Orange, damage) ) );
                }
            },

            ["exori vis"] = new Spell()
            {
                Name = "Energy Strike",

                Group = "Attack",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 20,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea, new SimpleAttack(null, null, AnimatedTextColor.LightBlue, damage) ) );
                }
            },
                       
            ["exevo flam hur"] = new Spell()
            {
                Name = "Fire Wave",

                Group = "Attack",

                CooldownInMilliseconds = 4000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 25,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                                                              new Offset(0, 1),
                                           new Offset(-1, 2), new Offset(0, 2), new Offset(1, 2),
                                           new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                        new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 30, 10);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.FireArea, new SimpleAttack(null, null, AnimatedTextColor.Orange, damage) ) );
                }
            },

            ["exevo vis lux"] = new Spell()
            {
                Name = "Energy Beam",

                Group = "Attack",

                CooldownInMilliseconds = 4000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 40,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1),
                        new Offset(0, 2),
                        new Offset(0, 3),
                        new Offset(0, 4),
                        new Offset(0, 5)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 60, 20);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea, new SimpleAttack(null, null, AnimatedTextColor.LightBlue, damage) ) );
                }
            },

            ["exevo gran vis lux"] = new Spell()
            {
                Name = "Great Energy Beam",

                Group = "Attack",

                CooldownInMilliseconds = 6000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 110,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1),
                        new Offset(0, 2),
                        new Offset(0, 3),
                        new Offset(0, 4),
                        new Offset(0, 5),
                        new Offset(0, 6),
                        new Offset(0, 7)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 120, 80);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea, new SimpleAttack(null, null, AnimatedTextColor.LightBlue, damage) ) );
                }
            },

            ["exevo mort hur"] = new Spell()
            {
                Name = "Great Energy Beam",

                Group = "Attack",

                CooldownInMilliseconds = 8000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 170,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                                            new Offset(0, 1),
                                            new Offset(0, 2),
                        new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                        new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
                        new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 150, 50);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.MortArea, new SimpleAttack(null, null, AnimatedTextColor.DarkRed, damage) ) );
                }
            },

            ["exevo gran mas vis"] = new Spell()
            {
                Name = "Rage of the Skies",

                Group = "Attack",

                CooldownInMilliseconds = 40000,

                GroupCooldownInMilliseconds = 4000,

                Premium = true,

                Mana = 600,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                                                                                                                            new Offset(0, -5),
                                                                                    new Offset(-2, -4), new Offset(-1, -4), new Offset(0, -4), new Offset(1, -4), new Offset(2, -4),
                                                                new Offset(-3, -3), new Offset(-2, -3), new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3), new Offset(2, -3), new Offset(3, -3),
                                            new Offset(-4, -2), new Offset(-3, -2), new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2), new Offset(3, -2), new Offset(4, -2),
                                            new Offset(-4, -1), new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1), new Offset(4, -1),
                         new Offset(-5, 0), new Offset(-4, 0),  new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),  new Offset(4, 0),  new Offset(5, 0),
                                            new Offset(-4, 1),  new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),  new Offset(4, 1),
                                            new Offset(-4, 2),  new Offset(-3, 2),  new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),  new Offset(3, 2),  new Offset(4, 2),
                                                                new Offset(-3, 3),  new Offset(-2, 3),  new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3),  new Offset(2, 3),  new Offset(3, 3),
                                                                                    new Offset(-2, 4),  new Offset(-1, 4),  new Offset(0, 4),  new Offset(1, 4),  new Offset(2, 4),
                                                                                                                            new Offset(0, 5),

                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 250, 50);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.FireArea, new SimpleAttack(null, null, AnimatedTextColor.Orange, damage) ) );
                }
            },

            ["exevo gran mas pox"] = new Spell()
            {
                Name = "Poison Storm",

                Group = "Attack",

                CooldownInMilliseconds = 40000,

                GroupCooldownInMilliseconds = 4000,

                Premium = true,

                Mana = 700,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                                                                                                                            new Offset(0, -5),
                                                                                    new Offset(-2, -4), new Offset(-1, -4), new Offset(0, -4), new Offset(1, -4), new Offset(2, -4),
                                                                new Offset(-3, -3), new Offset(-2, -3), new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3), new Offset(2, -3), new Offset(3, -3),
                                            new Offset(-4, -2), new Offset(-3, -2), new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2), new Offset(3, -2), new Offset(4, -2),
                                            new Offset(-4, -1), new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1), new Offset(4, -1),
                         new Offset(-5, 0), new Offset(-4, 0),  new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),  new Offset(4, 0),  new Offset(5, 0),
                                            new Offset(-4, 1),  new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),  new Offset(4, 1),
                                            new Offset(-4, 2),  new Offset(-3, 2),  new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),  new Offset(3, 2),  new Offset(4, 2),
                                                                new Offset(-3, 3),  new Offset(-2, 3),  new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3),  new Offset(2, 3),  new Offset(3, 3),
                                                                                    new Offset(-2, 4),  new Offset(-1, 4),  new Offset(0, 4),  new Offset(1, 4),  new Offset(2, 4),
                                                                                                                            new Offset(0, 5),

                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 200, 50);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.GreenRings, new SimpleAttack(null, null, AnimatedTextColor.Green, damage) ) );
                }
            },

            ["exori"] = new Spell()
            {
                Name = "Berserk",

                Group = "Attack",

                CooldownInMilliseconds = 4000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 115,

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0),                     new Offset(1, 0),
                        new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1)
                    };

                    var formula = BerserkFormula(attacker.Level, attacker.Skills.Fist, 0);

                    var damage = -Context.Current.Server.Randomization.Take(formula.Min, formula.Max);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark, new SimpleAttack(null, null, AnimatedTextColor.DarkRed, damage) ) );
                }
            }
        };

        private static ushort HasteFormula(ushort baseSpeed)
        {
            return (ushort)(baseSpeed * 1.3 - 24);
        }

        private static ushort StrongHasteFormula(ushort baseSpeed)
        {
            return (ushort)(baseSpeed * 1.7 - 56);
        }

        private static (int Min, int Max) LightHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 1.4 + 8), (int)(level * 0.2 + magicLevel * 1.795 + 11) );
        }

        private static (int Min, int Max) IntenseHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 3.184 + 20), (int)(level * 0.2 + magicLevel * 5.59 + 35) );
        }

        private static (int Min, int Max) MassHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 5.7 + 26), (int)(level * 0.2 + magicLevel * 10.43 + 62) );
        }

        private static (int Min, int Max) UltimateHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 7.22 + 44), (int)(level * 0.2 + magicLevel * 12.79 + 79) );
        }

        private static (int Min, int Max) BerserkFormula(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon) * 0.5 + level * 0.2), (int)( (skill + weapon) * 1.5 + level * 0.2) );
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

           return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            Spell spell;

            if (spells.TryGetValue(command.Message, out spell) )
            {
                PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.Components.GetComponent<PlayerCooldownBehaviour>(command.Player);

                if (command.Player.Mana >= spell.Mana)
                {
                    if ( !playerCooldownBehaviour.HasCooldown(spell.Name) && !playerCooldownBehaviour.HasCooldown(spell.Group) )
                    {
                        if (spell.Condition == null || spell.Condition(command.Player) )
                        {
                            playerCooldownBehaviour.AddCooldown(spell.Name, spell.CooldownInMilliseconds);
    
                            playerCooldownBehaviour.AddCooldown(spell.Group, spell.GroupCooldownInMilliseconds);

                            return Context.AddCommand(new PlayerUpdateManaCommand(command.Player, command.Player.Mana - spell.Mana) ).Then( () =>
                            {
                                return spell.Callback(command.Player);

                            } ).Then( () =>
                            {
                                return next();
                            } );
                        }

                        return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                        {
                            Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

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
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMana) );
                         
                    return Promise.Break;
                } );
            }

            return next();
        }
    }
}