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

            public TimeSpan Cooldown { get; set; }

            public TimeSpan GroupCooldown { get; set; }

            public int Level { get; set; }

            public int Mana { get; set; }

            public bool Premium { get; set; }

            public Vocation[] Vocations { get; set; }

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

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 9,

                Mana = 20,

                Premium = true,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

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
                        return Context.Current.AddCommand(new CreatureWalkCommand(attacker, toTile, Direction.South) );

                    } ).Then( () =>
                    {
                        return Context.Current.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                    } );
                }
            },

            ["exani hur up"] = new Spell()
            {
                Name = "Levitate",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 12,

                Mana = 50,

                Premium = true,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Condition = (attacker) =>
                {
                    Tile up = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1) );

                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1).Offset(attacker.Direction) );

                    if (up != null || toTile == null || toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
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
                        return Context.Current.AddCommand(new CreatureWalkCommand(attacker, toTile) );

                    } ).Then( () =>
                    {
                        return Context.Current.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                    } );
                }
            },

            ["exani hur down"] = new Spell()
            {
                Name = "Levitate",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 12,

                Mana = 50,

                Premium = true,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Condition = (attacker) =>
                {
                    Tile next = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(attacker.Direction) );

                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, 1).Offset(attacker.Direction) );

                    if (next != null || toTile == null || toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
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
                        return Context.Current.AddCommand(new CreatureWalkCommand(attacker, toTile) );

                    } ).Then( () =>
                    {
                        return Context.Current.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                    } );
                }
            },

            ["utevo lux"] = new Spell()
            {
                Name = "Light",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 8,

                Mana = 20,

                Premium = false,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                            new LightCondition(new Light(6, 215), new TimeSpan(0, 6, 10) ) ) );
                    } );
                }
            },

            ["utevo gran lux"] = new Spell()
            {
                Name = "Great Light",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 13,

                Mana = 60,

                Premium = false,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                            new LightCondition(new Light(8, 215), new TimeSpan(0, 11, 35) ) ) );
                    } );
                }
            },

            ["utevo vis lux"] = new Spell()
            {
                Name = "Ultimate Light",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 26,

                Mana = 140,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                            new LightCondition(new Light(9, 215), new TimeSpan(0, 33, 10) ) ) );
                    } );
                }
            },

            ["utana vid"] = new Spell()
            {
                Name = "Invisible",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 35,

                Mana = 440,

                Premium = false,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                            new InvisibleCondition(new TimeSpan(0, 3, 20) ) ) );
                    } );
                }
            },

            ["utani hur"] = new Spell()
            {
                Name = "Haste",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 14,

                Mana = 60,

                Premium = true,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    var speed = HasteFormula(attacker.BaseSpeed);

                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                            new HasteCondition(speed, new TimeSpan(0, 0, 33) ) ) );
                    } );
                }
            },

            ["utani gran hur"] = new Spell()
            {
                Name = "Strong Haste",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 20,

                Mana = 100,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    var speed = StrongHasteFormula(attacker.BaseSpeed);

                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                            new HasteCondition(speed, new TimeSpan(0, 0, 22) ) ) );
                    } );
                }
            },
       
            ["utamo vita"] = new Spell()
            {
                Name = "Magic Shield",

                Group = "Support",

                Cooldown = TimeSpan.FromSeconds(14),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 14,

                Mana = 50,

                Premium = false,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
                    {
                        return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                            new MagicShieldCondition(new TimeSpan(0, 3, 0) ) ) );
                    } );
                }
            },

            ["exana pox"] = new Spell()
            {
                Name = "Cure Poison",

                Group = "Healing",

                Cooldown = TimeSpan.FromSeconds(6),

                GroupCooldown = TimeSpan.FromSeconds(1),

                Level = 10,

                Mana = 30,

                Premium = false,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

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

                Cooldown = TimeSpan.FromSeconds(1),

                GroupCooldown = TimeSpan.FromSeconds(1),

                Level = 9,

                Mana = 20,

                Premium = false,

                Vocations = new[] { Vocation.Knight, Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.EliteKnight, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    var formula = LightHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, 
                        
                        new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
                }
            },

            ["exura gran"] = new Spell()
            {
                Name = "Intense Healing",

                Group = "Healing",

                Cooldown = TimeSpan.FromSeconds(1),

                GroupCooldown = TimeSpan.FromSeconds(1),

                Level = 11,

                Mana = 70,

                Premium = false,

                Vocations = new[] { Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    var formula = IntenseHealingFormula(attacker.Level, attacker.Skills.MagicLevel);
                    
                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, 
                        
                        new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
                }
            },

            ["exura vita"] = new Spell()
            {
                Name = "Ultimate Healing",

                Group = "Healing",

                Cooldown = TimeSpan.FromSeconds(1),

                GroupCooldown = TimeSpan.FromSeconds(1),

                Level = 20,

                Mana = 160,

                Premium = false,

                Vocations = new[] { Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    var formula = UltimateHealingFormula(attacker.Level, attacker.Skills.MagicLevel);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, 
                        
                        new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
                }
            },

            ["exura gran mas res"] = new Spell()
            {
                Name = "Mass Healing",

                Group = "Healing",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(1),

                Level = 36,

                Mana = 150,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlueShimmer, 
                        
                        new HealingAttack(null, formula.Min, formula.Max) ) );
                }
            },

            ["exori mort"] = new Spell()
            {
                Name = "Death Strike",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 16,

                Mana = 20,

                Premium = true,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.MortArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
            },

            ["exori flam"] = new Spell()
            {
                Name = "Flame Strike",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 12,

                Mana = 20,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.FirePlume, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
                }
            },

            ["exori vis"] = new Spell()
            {
                Name = "Energy Strike",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(2),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 12,

                Mana = 20,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 45, 10);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
                }
            },
                       
            ["exevo flam hur"] = new Spell()
            {
                Name = "Fire Wave",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(4),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 18,

                Mana = 25,

                Premium = false,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.FireArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
                }
            },

            ["exevo vis lux"] = new Spell()
            {
                Name = "Energy Beam",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(4),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 23,

                Mana = 40,

                Premium = false,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea,
                        
                        new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
                }
            },

            ["exevo gran vis lux"] = new Spell()
            {
                Name = "Great Energy Beam",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(6),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 29,
                
                Mana = 110,

                Premium = false,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
                }
            },

            ["exevo mort hur"] = new Spell()
            {
                Name = "Great Energy Beam",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(8),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 38,

                Mana = 170,

                Premium = false,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.MortArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
            },

            ["exevo gran mas vis"] = new Spell()
            {
                Name = "Rage of the Skies",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(40),

                GroupCooldown = TimeSpan.FromSeconds(4),

                Level = 55,

                Mana = 600,

                Premium = true,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.FireArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
                }
            },

            ["exevo gran mas pox"] = new Spell()
            {
                Name = "Poison Storm",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(40),

                GroupCooldown = TimeSpan.FromSeconds(4),

                Level = 60,

                Mana = 700,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

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

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.GreenRings, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Green, formula.Min, formula.Max) ) );
                }
            },

            ["exori mas"] = new Spell()
            {
                Name = "Groundshaker",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(8),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 33,

                Mana = 160,

                Premium = true,

                Vocations = new[] { Vocation.Knight, Vocation.EliteKnight },

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

                    Item itemWeapon = GetWeapon(attacker);

                    if (itemWeapon == null)
                    {
                        var formula = GroundshakerFormula(attacker.Level, attacker.Skills.Fist, 7);

                        return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                            new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                    }
                    else
                    {
                        if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                        {
                            var formula = GroundshakerFormula(attacker.Level, attacker.Skills.Sword, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                        {
                            var formula = GroundshakerFormula(attacker.Level, attacker.Skills.Axe, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                        {
                            var formula = GroundshakerFormula(attacker.Level, attacker.Skills.Club, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            },

            ["exori"] = new Spell()
            {
                Name = "Berserk",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(4),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 35,

                Mana = 115,

                Premium = true,

                Vocations = new[] { Vocation.Knight, Vocation.EliteKnight },

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                        new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1)
                    };

                    Item itemWeapon = GetWeapon(attacker);

                    if (itemWeapon == null)
                    {
                        var formula = BerserkFormula(attacker.Level, attacker.Skills.Fist, 7);

                        return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                            new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                    }
                    else
                    {
                        if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                        {
                            var formula = BerserkFormula(attacker.Level, attacker.Skills.Sword, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                        {
                            var formula = BerserkFormula(attacker.Level, attacker.Skills.Axe, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                        {
                            var formula = BerserkFormula(attacker.Level, attacker.Skills.Club, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            },

            ["exori gran"] = new Spell()
            {
                Name = "Fierce Berserk",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(6),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 70,

                Mana = 340,

                Premium = true,

                Vocations = new[] { Vocation.Knight, Vocation.EliteKnight },

                Callback = (attacker) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                        new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1)
                    };

                    Item itemWeapon = GetWeapon(attacker);

                    if (itemWeapon == null)
                    {
                        var formula = FierceBerserkFormula(attacker.Level, attacker.Skills.Fist, 7);

                        return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                            new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                    }
                    else
                    {
                        if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                        {
                            var formula = FierceBerserkFormula(attacker.Level, attacker.Skills.Sword, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                        {
                            var formula = FierceBerserkFormula(attacker.Level, attacker.Skills.Axe, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                        {
                            var formula = FierceBerserkFormula(attacker.Level, attacker.Skills.Club, itemWeapon.Metadata.Attack.Value);

                            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
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

        private static (int Min, int Max) GroundshakerFormula(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon) * 0.5 + level * 0.2), (int)( (skill + weapon) * 1.1 + level * 0.2) );
        }

        private static (int Min, int Max) BerserkFormula(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon) * 0.5 + level * 0.2), (int)( (skill + weapon) * 1.5 + level * 0.2) );
        }

        private static (int Min, int Max) FierceBerserkFormula(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon * 2) * 1.1 + level * 0.2), (int)( (skill + weapon * 2) * 3 + level * 0.2) );
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

           return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        private static Item GetWeapon(Player player)
        {
            Item item = player.Inventory.GetContent( (byte)Slot.Left) as Item;

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe) )
            {
                return item;
            }

            item = player.Inventory.GetContent( (byte)Slot.Right) as Item;

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe) )
            {
                return item;
            }

            return null;
        }

        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            Spell spell;

            if (spells.TryGetValue(command.Message, out spell) )
            {
                if (command.Player.Level < spell.Level)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughLevel) );

                        return Promise.Break;
                    } );
                }
                 
                if (command.Player.Mana < spell.Mana)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMana) );

                        return Promise.Break;
                    } );
                }

                if (spell.Vocations != null && !spell.Vocations.Contains(command.Player.Vocation) )
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YourVocationCannotUseThisSpell) );

                        return Promise.Break;
                    } );
                }
                 
                if (spell.Group == "Attack" && command.Player.Tile.ProtectionZone)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisActionIsNotPermittedInAProtectionZone) );

                        return Promise.Break;
                    } );
                }

                PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(command.Player);

                if (playerCooldownBehaviour.HasCooldown(spell.Name) || playerCooldownBehaviour.HasCooldown(spell.Group) )
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );

                        return Promise.Break;
                    } );
                }

                if (spell.Condition != null && !spell.Condition(command.Player) )
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                        return Promise.Break;
                    } );
                }

                playerCooldownBehaviour.AddCooldown(spell.Name, spell.Cooldown);

                playerCooldownBehaviour.AddCooldown(spell.Group, spell.GroupCooldown);

                return Context.AddCommand(new PlayerUpdateManaCommand(command.Player, command.Player.Mana - spell.Mana) ).Then( () =>
                {
                    return spell.Callback(command.Player);

                } ).Then( () =>
                {
                    return next();
                } );
            }

            return next();
        }
    }
}