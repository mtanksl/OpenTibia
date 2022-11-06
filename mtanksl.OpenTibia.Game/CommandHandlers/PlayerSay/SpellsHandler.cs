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

                Condition = (context, player) =>
                {
                    if (ropeSpots.Contains(player.Tile.Ground.Metadata.OpenTibiaId) )
                    {
                        return true;
                    }

                    return false;
                },

                Callback = Teleport()
            },

            ["exani hur up"] = new Spell()
            {
                Name = "Levitate",
                
                Group = "Support", 
                
                CooldownInMilliseconds = 2000,
                
                GroupCooldownInMilliseconds = 2000, 
                
                Premium = true,

                Mana = 50,

                Condition = (context, player) =>
                {
                    Tile up = context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, -1) );

                    Tile toTile = context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, -1).Offset(player.Direction) );

                    if (up != null || toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = TeleportUp()
            },

            ["exani hur down"] = new Spell()
            {
                Name = "Levitate",
                
                Group = "Support", 
                
                CooldownInMilliseconds = 2000,
                
                GroupCooldownInMilliseconds = 2000, 
                
                Premium = true,

                Mana = 50,

                Condition = (context, player) =>
                {
                    Tile next = context.Server.Map.GetTile(player.Tile.Position.Offset(player.Direction) );

                    Tile toTile = context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, 1).Offset(player.Direction) );

                    if (next != null || toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                    {
                        return false;
                    }

                    return true;
                },

                Callback = TeleportDown()
            },

            ["utevo lux"] = new Spell()
            {
                Name = "Light",
                
                Group = "Support", 
                
                CooldownInMilliseconds = 2000,
                
                GroupCooldownInMilliseconds = 2000, 
                
                Premium = false,

                Mana = 20,

                Callback = Light(6, 215)
            },

            ["utevo gran lux"] = new Spell()
            {
                Name = "Great Light",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 60,

                Callback = Light(8, 215)
            },

            ["utevo vis lux"] = new Spell()
            {
                Name = "Ultimate Light",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 140,

                Callback = Light(9, 215)
            },

            ["utani hur"] = new Spell()
            {
                Name = "Haste",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 60,

                Callback = Speed(player => HasteFormula(player.BaseSpeed) )
            },

            ["utani gran hur"] = new Spell()
            {
                Name = "Strong Haste",

                Group = "Support",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 100,

                Callback = Speed(player => StrongHasteFormula(player.BaseSpeed) )
            },

            ["exura"] = new Spell()
            {
                Name = "Light Healing",

                Group = "Healing",

                CooldownInMilliseconds = 1000,

                GroupCooldownInMilliseconds = 1000,

                Premium = false,

                Mana = 20,

                Callback = Healing(player => LightHealingFormula(player.Level, player.Skills.MagicLevel) )
            },

            ["exura gran"] = new Spell()
            {
                Name = "Intense Healing",

                Group = "Healing",

                CooldownInMilliseconds = 1000,

                GroupCooldownInMilliseconds = 1000,

                Premium = false,

                Mana = 70,

                Callback = Healing(player => IntenseHealingFormula(player.Level, player.Skills.MagicLevel) )
            },

            ["exura vita"] = new Spell()
            {
                Name = "Ultimate Healing",

                Group = "Healing",

                CooldownInMilliseconds = 1000,

                GroupCooldownInMilliseconds = 1000,

                Premium = false,

                Mana = 160,

                Callback = Healing(player => UltimateHealingFormula(player.Level, player.Skills.MagicLevel) )
            },

            ["exura gran mas res"] = new Spell()
            {
                Name = "Mass Healing",

                Group = "Healing",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 1000,

                Premium = true,

                Mana = 150,

                Callback = Healing(new Offset[]
                {
                                                            new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
                                        new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
                    new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
                    new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
                    new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
                                        new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
                                                            new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3)

                }, player => MassHealingFormula(player.Level, player.Skills.MagicLevel) )
            },

            ["exori mort"] = new Spell()
            {
                Name = "Death Strike",

                Group = "Attack",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 20,

                Callback = BeamAttack(new Offset[]
                {
                    new Offset(0, 1)

                }, MagicEffectType.MortArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10) )
            },

            ["exori flam"] = new Spell()
            {
                Name = "Flame Strike",

                Group = "Attack",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 20,

                Callback = BeamAttack(new Offset[]
                {
                    new Offset(0, 1)

                }, MagicEffectType.FirePlume, player => GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10) )
            },

            ["exori vis"] = new Spell()
            {
                Name = "Energy Strike",

                Group = "Attack",

                CooldownInMilliseconds = 2000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 20,

                Callback = BeamAttack(new Offset[]
                {
                    new Offset(0, 1)

                }, MagicEffectType.EnergyArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10) )
            },

            ["exevo flam hur"] = new Spell()
            {
                Name = "Fire Wave",

                Group = "Attack",

                CooldownInMilliseconds = 4000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 25,

                Callback = BeamAttack(new Offset[]
                {
                                                          new Offset(0, 1),
                                       new Offset(-1, 2), new Offset(0, 2), new Offset(1, 2),
                                       new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                    new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4)

                }, MagicEffectType.FireArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 30, 10) )
            },

            ["exevo vis lux"] = new Spell()
            {
                Name = "Energy Beam",

                Group = "Attack",

                CooldownInMilliseconds = 4000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 40,

                Callback = BeamAttack(new Offset[]
                {
                    new Offset(0, 1),
                    new Offset(0, 2),
                    new Offset(0, 3),
                    new Offset(0, 4),
                    new Offset(0, 5)

                }, MagicEffectType.EnergyArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 60, 20) )
            },

            ["exevo gran vis lux"] = new Spell()
            {
                Name = "Great Energy Beam",

                Group = "Attack",

                CooldownInMilliseconds = 6000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 110,

                Callback = BeamAttack(new Offset[]
                {
                    new Offset(0, 1),
                    new Offset(0, 2),
                    new Offset(0, 3),
                    new Offset(0, 4),
                    new Offset(0, 5),
                    new Offset(0, 6),
                    new Offset(0, 7)

                }, MagicEffectType.EnergyArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 120, 80) )
            },

            ["exevo mort hur"] = new Spell()
            {
                Name = "Great Energy Beam",

                Group = "Attack",

                CooldownInMilliseconds = 8000,

                GroupCooldownInMilliseconds = 2000,

                Premium = false,

                Mana = 170,

                Callback = BeamAttack(new Offset[]
                {
                                        new Offset(0, 1),
                                        new Offset(0, 2),
                    new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                    new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
                    new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),

                }, MagicEffectType.MortArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 150, 50) )
            },

            ["exevo gran mas vis"] = new Spell()
            {
                Name = "Rage of the Skies",

                Group = "Attack",

                CooldownInMilliseconds = 40000,

                GroupCooldownInMilliseconds = 4000,

                Premium = true,

                Mana = 600,

                Callback = AreaAttack(new Offset[]
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

                }, MagicEffectType.FireArea, player => GenericFormula(player.Level, player.Skills.MagicLevel, 250, 50) )
            },

            ["exevo gran mas pox"] = new Spell()
            {
                Name = "Poison Storm",

                Group = "Attack",

                CooldownInMilliseconds = 40000,

                GroupCooldownInMilliseconds = 4000,

                Premium = true,

                Mana = 700,

                Callback = AreaAttack(new Offset[]
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

                }, MagicEffectType.GreenRings, player => GenericFormula(player.Level, player.Skills.MagicLevel, 200, 50) )
            },

            ["exori"] = new Spell()
            {
                Name = "Berserk",

                Group = "Attack",

                CooldownInMilliseconds = 4000,

                GroupCooldownInMilliseconds = 2000,

                Premium = true,

                Mana = 115,

                Callback = AreaAttack(new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0),                     new Offset(1, 0),
                    new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1)

                }, MagicEffectType.BlackSpark, player => BerserkFormula(player.Level, player.Skills.Sword, 0) )
            }
        };

        private static Func<Context, Player, Promise> Teleport()
        {
            return (context, player) =>
            {
                Tile toTile = context.Server.Map.GetTile(player.Tile.Position.Offset(0, 1, -1) );

                return context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.Teleport) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateParentCommand(player, toTile, Direction.South) );
                } );
            };
        }

        private static Func<Context, Player, Promise> TeleportUp()
        {
            return (context, player) =>
            {
                Tile toTile = context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, -1).Offset(player.Direction) );

                return context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.Teleport) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateParentCommand(player, toTile) );
                } );
            };
        }

        private static Func<Context, Player, Promise> TeleportDown()
        {
            return (context, player) =>
            {
                Tile toTile = context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, 1).Offset(player.Direction) );

                return context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.Teleport) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateParentCommand(player, toTile) );
                } );
            };
        }

        private static Func<Context, Player, Promise> Light(byte level, byte color)
        {
            return (context, player) =>
            {
                return context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.BlueShimmer) ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateLightCommand(player, new Light(level, color) ) );
                } );
            };
        }

        private static Func<Context, Player, Promise> Speed(Func<Player, ushort> formula)
        {
            return (context, player) =>
            {
                return context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.GreenShimmer) ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateSpeedCommand(player, formula(player) ) );
                } );
            };           
        }

        private static Func<Context, Player, Promise> Healing(Func<Player, (int Min, int Max)> formula)
        {
            return (context, player) =>
            {
                var calculated = formula(player);

                return context.AddCommand(CombatCommand.TargetAttack(player, player, null, MagicEffectType.BlueShimmer, (attacker, target) => context.Server.Randomization.Take(calculated.Min, calculated.Max) ) );
            };           
        }

        private static Func<Context, Player, Promise> Healing(Offset[] area, Func<Player, (int Min, int Max)> formula)
        {
            return (context, player) =>
            {
                var calculated = formula(player);

                return context.AddCommand(CombatCommand.AreaAttack(player, area, null, MagicEffectType.BlueShimmer, (attacker, target) => context.Server.Randomization.Take(calculated.Min, calculated.Max) ) );
            };           
        }

        private static Func<Context, Player, Promise> AreaAttack(Offset[] area, MagicEffectType? magicEffectType, Func<Player, (int Min, int Max)> formula)
        {
            return (context, player) =>
            {
                var calculated = formula(player);

                return context.AddCommand(CombatCommand.AreaAttack(player, area, null, magicEffectType, (attacker, target) => -context.Server.Randomization.Take(calculated.Min, calculated.Max) ) );
            };
        }

        private static Func<Context, Player, Promise> BeamAttack(Offset[] beam, MagicEffectType? magicEffectType, Func<Player, (int Min, int Max)> formula)
        {
            return (context, player) =>
            {
                var calculated = formula(player);

                return context.AddCommand(CombatCommand.BeamAttack(player, beam, magicEffectType, (attacker, target) => -context.Server.Randomization.Take(calculated.Min, calculated.Max) ) );
            };
        }

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

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerSayCommand command)
        {
            Spell spell;

            if (spells.TryGetValue(command.Message, out spell) )
            {
                CooldownBehaviour component = context.Server.Components.GetComponent<CooldownBehaviour>(command.Player);

                if (command.Player.Mana >= spell.Mana)
                {
                    if ( !component.HasCooldown(spell.Name) && !component.HasCooldown(spell.Group) )
                    {
                        if (spell.Condition == null || spell.Condition(context, command.Player) )
                        {
                            component.AddCooldown(spell.Name, spell.CooldownInMilliseconds);
    
                            component.AddCooldown(spell.Group, spell.GroupCooldownInMilliseconds);

                            return next(context).Then(ctx =>
                            {
                                return spell.Callback(ctx, command.Player);
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
                else
                {
                    return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) ).Then(ctx =>
                    {
                        ctx.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMana) );
                    } );
                }
            }

            return next(context);
        }
    }
}