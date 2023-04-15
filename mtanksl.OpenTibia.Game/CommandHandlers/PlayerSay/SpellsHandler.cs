﻿using OpenTibia.Common.Objects;
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

            public Func<Context, Player, bool> Condition { get; set; }

            public Func<Context, Player, Promise> Callback { get; set; }
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

                Condition = (context, attacker) =>
                {
                    if (ropeSpots.Contains(attacker.Tile.Ground.Metadata.OpenTibiaId) )
                    {
                        return true;
                    }

                    return false;
                },

                Callback = (context, attacker) =>
                {
                    Tile toTile = context.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 1, -1) );

                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.Teleport) ).Then(() =>
                    {
                        return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                    } ).Then(() =>
                    {
                        return context.AddCommand(new CreatureUpdateParentCommand(attacker, toTile, Direction.South) );
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

                Condition = (context, attacker) =>
                {
                    Tile up = context.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1) );

                    Tile toTile = context.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1).Offset(attacker.Direction) );

                    if (up != null || toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker) =>
                {
                    Tile toTile = context.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1).Offset(attacker.Direction) );

                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.Teleport) ).Then(() =>
                    {
                        return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                    } ).Then(() =>
                    {
                        return context.AddCommand(new CreatureUpdateParentCommand(attacker, toTile) );
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

                Condition = (context, attacker) =>
                {
                    Tile next = context.Server.Map.GetTile(attacker.Tile.Position.Offset(attacker.Direction) );

                    Tile toTile = context.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, 1).Offset(attacker.Direction) );

                    if (next != null || toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = (context, attacker) =>
                {
                    Tile toTile = context.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, 1).Offset(attacker.Direction) );

                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.Teleport) ).Then(() =>
                    {
                        return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                    } ).Then(() =>
                    {
                        return context.AddCommand(new CreatureUpdateParentCommand(attacker, toTile) );
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

                Callback = (context, attacker) =>
                {
                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then(() =>
                    {
                        return context.AddCommand(new CreatureUpdateLightCommand(attacker, new Light(6, 215) ) );
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

                Callback = (context, attacker) =>
                {
                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then(() =>
                    {
                        return context.AddCommand(new CreatureUpdateLightCommand(attacker, new Light(8, 215) ) );
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

                Callback = (context, attacker) =>
                {
                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then(() =>
                    {
                        return context.AddCommand(new CreatureUpdateLightCommand(attacker, new Light(9, 215) ) );
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

                Callback = (context, attacker) =>
                {
                    var speed = HasteFormula(attacker.BaseSpeed);

                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
                    {
                        return context.AddCommand(new CreatureUpdateSpeedCommand(attacker, speed) );
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

                Callback = (context, attacker) =>
                {
                    var speed = StrongHasteFormula(attacker.BaseSpeed);

                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
                    {
                        return context.AddCommand(new CreatureUpdateSpeedCommand(attacker, speed) );
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

                Callback = (context, attacker) =>
                {
                    return context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        SpecialConditionBehaviour component = context.Server.Components.GetComponent<SpecialConditionBehaviour>(attacker);

                        if (component != null)
                        {
                            if (component.HasSpecialCondition(SpecialCondition.Poisoned) )
                            {
                                component.RemoveSpecialCondition(SpecialCondition.Poisoned);

                                context.AddPacket(attacker.Client.Connection, new SetSpecialConditionOutgoingPacket(component.SpecialConditions) );
                            }

                            context.Server.CancelQueueForExecution("Combat_Condition_" + SpecialCondition.Poisoned + attacker.Id);
                        }
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

                Callback = (context, attacker) =>
                {
                    var damage = LightHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    return context.AddCommand(new CombatAttackCreatureWithRuneOrSpellCommand(attacker, attacker, null, MagicEffectType.BlueShimmer, (attacker, target) => context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    var damage = IntenseHealingFormula(attacker.Level, attacker.Skills.MagicLevel);
                    
                    return context.AddCommand(new CombatAttackCreatureWithRuneOrSpellCommand(attacker, attacker, null, MagicEffectType.BlueShimmer, (attacker, target) => context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    var damage = UltimateHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    return context.AddCommand(new CombatAttackCreatureWithRuneOrSpellCommand(attacker, attacker, null, MagicEffectType.BlueShimmer, (attacker, target) => context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
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

                    var damage = MassHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsRadialCommand(attacker, area, MagicEffectType.BlueShimmer, (attacker, target) => context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.MortArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.FirePlume, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.EnergyArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                                                              new Offset(0, 1),
                                           new Offset(-1, 2), new Offset(0, 2), new Offset(1, 2),
                                           new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                        new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 30, 10);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.FireArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1),
                        new Offset(0, 2),
                        new Offset(0, 3),
                        new Offset(0, 4),
                        new Offset(0, 5)
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 60, 20);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.EnergyArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
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

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 120, 80);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.EnergyArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                                            new Offset(0, 1),
                                            new Offset(0, 2),
                        new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                        new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
                        new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),
                    };

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 150, 50);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.MortArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
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

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 250, 50);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.FireArea, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
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

                    var damage = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 200, 50);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsBeamCommand(attacker, area, MagicEffectType.GreenRings, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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

                Callback = (context, attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0),                     new Offset(1, 0),
                        new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1)
                    };

                    var damage = BerserkFormula(attacker.Level, attacker.Skills.Sword, 0);

                    return context.AddCommand(new CombatAttackAreaWithSpellAsRadialCommand(attacker, area, MagicEffectType.BlackSpark, (attacker, target) => -context.Server.Randomization.Take(damage.Min, damage.Max) ) );
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
                CooldownBehaviour component = Context.Server.Components.GetComponent<CooldownBehaviour>(command.Player);

                if (command.Player.Mana >= spell.Mana)
                {
                    if ( !component.HasCooldown(spell.Name) && !component.HasCooldown(spell.Group) )
                    {
                        if (spell.Condition == null || spell.Condition(Context, command.Player) )
                        {
                            component.AddCooldown(spell.Name, spell.CooldownInMilliseconds);
    
                            component.AddCooldown(spell.Group, spell.GroupCooldownInMilliseconds);

                            return Context.AddCommand(new PlayerUpdateManaCommand(command.Player, command.Player.Mana - spell.Mana) ).Then( () =>
                            {
                                return spell.Callback(Context, command.Player);

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