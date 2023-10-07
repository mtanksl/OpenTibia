using mtanksl.OpenTibia.Game.Plugins;
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

                Condition = (attacker, message) =>
                {
                    if (ropeSpots.Contains(attacker.Tile.Ground.Metadata.OpenTibiaId) )
                    {
                        return Promise.FromResult(true);
                    }

                    return Promise.FromResult(false);
                },

                Callback = (attacker, message) =>
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

                Condition = (attacker, message) =>
                {
                    Tile up = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1) );

                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, -1).Offset(attacker.Direction) );

                    if (up != null || toTile == null || toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return Promise.FromResult(false);
                    }

                    return Promise.FromResult(true);
                },

                Callback = (attacker, message) =>
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

                Condition = (attacker, message) =>
                {
                    Tile next = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(attacker.Direction) );

                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(0, 0, 1).Offset(attacker.Direction) );

                    if (next != null || toTile == null || toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return Promise.FromResult(false);
                    }

                    return Promise.FromResult(true);
                },

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Level = 8,

                Mana = 20,

                Premium = false,

                Vocations = new[] { Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker, message) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 1.4, 8, 1.795, 11);

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

                Level = 20,

                Mana = 70,

                Premium = false,

                Vocations = new[] { Vocation.Paladin, Vocation.Druid, Vocation.Sorcerer, Vocation.RoyalPaladin, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker, message) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 3.184, 20, 5.59, 35);
                    
                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, 
                        
                        new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
                }
            },

            ["exura ico"] = new Spell()
            {
                Name = "Wound Cleansing",

                Group = "Healing",

                Cooldown = TimeSpan.FromSeconds(1),

                GroupCooldown = TimeSpan.FromSeconds(1),

                Level = 8,

                Mana = 40,

                Premium = false,

                Vocations = new[] { Vocation.Knight, Vocation.EliteKnight },

                Callback = (attacker, message) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 4, 25, 7.95, 51);
                    
                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker, 
                        
                        new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
                }
            },

            ["exura san"] = new Spell()
            {
                Name = "Divine Healing",

                Group = "Healing",

                Cooldown = TimeSpan.FromSeconds(1),

                GroupCooldown = TimeSpan.FromSeconds(1),

                Level = 35,

                Mana = 160,

                Premium = false,

                Vocations = new[] { Vocation.Paladin, Vocation.RoyalPaladin },

                Callback = (attacker, message) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 18.5, 0, 25, 0);
                    
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

                Level = 30,

                Mana = 160,

                Premium = false,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker, message) =>
                {
                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 7.22, 44, 12.79, 79);

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

                Callback = (attacker, message) =>
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 5.7, 26, 10.43, 62);

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

                Callback = (attacker, message) =>
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

                Level = 14,

                Mana = 20,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.Sorcerer, Vocation.ElderDruid, Vocation.MasterSorcerer },

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
                {
                    Offset[] area = new Offset[]
                    {
                                                              new Offset(0, 1),
                                           new Offset(-1, 2), new Offset(0, 2), new Offset(1, 2),
                                           new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                        new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 1.2, 0, 2, 0);

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

                Callback = (attacker, message) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(0, 1),
                        new Offset(0, 2),
                        new Offset(0, 3),
                        new Offset(0, 4),
                        new Offset(0, 5)
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 2.5, 0, 4, 0);

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

                Callback = (attacker, message) =>
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 4, 0, 7, 0);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
                }
            },

            ["exevo vis hur"] = new Spell()
            {
                Name = "Energy Wave",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(8),

                GroupCooldown = TimeSpan.FromSeconds(2),

                Level = 38,

                Mana = 170,

                Premium = false,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                Callback = (attacker, message) =>
                {
                    Offset[] area = new Offset[]
                    {
                                           new Offset(0, 1),
                                           new Offset(0, 2),
                        new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                        new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
                        new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),
                    };

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 4.5, 0, 9, 0);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
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

                Callback = (attacker, message) =>
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 5, 0, 12, 0);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.BigClouds, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
                }
            },

            ["exevo gran mas flam"] = new Spell()
            {
                Name = "Hell's Core",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(40),

                GroupCooldown = TimeSpan.FromSeconds(4),

                Level = 60,

                Mana = 1100,

                Premium = true,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                Callback = (attacker, message) =>
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 7, 0, 14, 0);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.FireArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
                }
            },

            ["exevo gran mas tera"] = new Spell()
            {
                Name = "Wrath of Nature",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(40),

                GroupCooldown = TimeSpan.FromSeconds(4),

                Level = 55,

                Mana = 700,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

                Callback = (attacker, message) =>
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 5, 0, 10, 0);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.PlantAttack, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Green, formula.Min, formula.Max) ) );
                }
            },

            ["exevo gran mas frigo"] = new Spell()
            {
                Name = "Eternal Winter",

                Group = "Attack",

                Cooldown = TimeSpan.FromSeconds(40),

                GroupCooldown = TimeSpan.FromSeconds(4),

                Level = 60,

                Mana = 1050,

                Premium = true,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

                Callback = (attacker, message) =>
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

                    var formula = GenericFormula(attacker.Level, attacker.Skills.MagicLevel, 6, 0, 12, 0);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, MagicEffectType.IceTornado, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Crystal, formula.Min, formula.Max) ) );
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

                Callback = (attacker, message) =>
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

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, double minx, double miny, double maxx, double maxy)
        {
            return ( (int)(level * 0.2 + magicLevel * minx + miny), (int)(level * 0.2 + magicLevel * maxx + maxy) );
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

        public override async Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            Spell spell;

            if ( !spells.TryGetValue(command.Message, out spell) )
            {
                SpellPlugin plugin = Context.Server.Plugins.GetSpellPlugin(command.Message);

                if (plugin != null)
                {
                    spell = plugin.Spell;
                }
            }

            if (spell != null)
            {
                if (command.Player.Level < spell.Level)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughLevel) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }
                 
                if (command.Player.Mana < spell.Mana)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMana) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (spell.Vocations != null && !spell.Vocations.Contains(command.Player.Vocation) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YourVocationCannotUseThisSpell) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }
                 
                if (spell.Group == "Attack" && command.Player.Tile.ProtectionZone)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisActionIsNotPermittedInAProtectionZone) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(command.Player);

                if (playerCooldownBehaviour.HasCooldown(spell.Name) || playerCooldownBehaviour.HasCooldown(spell.Group) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (spell.Condition != null && !await spell.Condition(command.Player, command.Message) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                playerCooldownBehaviour.AddCooldown(spell.Name, spell.Cooldown);

                playerCooldownBehaviour.AddCooldown(spell.Group, spell.GroupCooldown);

                await Context.AddCommand(new PlayerUpdateManaCommand(command.Player, command.Player.Mana - spell.Mana) );

                await spell.Callback(command.Player, command.Message);
            }

            await next();
        }
    }
}