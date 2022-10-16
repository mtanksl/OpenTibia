using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpellsHandler : CommandHandler<PlayerSayCommand>
    {
        private static Dictionary<string, Func<Player, Func<Context, Promise>>> spells = new Dictionary<string, Func<Player, Func<Context, Promise>>>()
        {
            { "utevo lux", player =>
            {
                return Light(player, 6, 215);
            } },

            { "utevo gran lux", player => 
            {
                return Light(player, 8, 215); 
            } },

            { "utevo vis lux", player => 
            {
                return Light(player, 9, 215); 
            } },

            { "utani hur", player =>
            {
                return Speed(player, HasteFormula(player.BaseSpeed) );
            } },

            { "utani gran hur", player =>
            {
                return Speed(player, StrongHasteFormula(player.BaseSpeed) );
            } },

            { "exura", player =>
            {
                return Healing(player, LightHealingFormula(player.Level, player.Skills.MagicLevel) );
            } },

            { "exura gran", player =>
            {
                return Healing(player, IntenseHealingFormula(player.Level, player.Skills.MagicLevel) );
            } },

            { "exura vita", player =>
            {
                return Healing(player, UltimateHealingFormula(player.Level, player.Skills.MagicLevel) );
            } },

            { "exura gran mas res", player =>
            {
                var area = new Offset[]
                {
                                                            new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
                                        new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
                    new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
                    new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
                    new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
                                        new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
                                                            new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3)
                };

                return Healing(player, area, MassHealingFormula(player.Level, player.Skills.MagicLevel) );
            } },

            { "exori mort", player =>
            {
                var beam = new Offset[] 
                {
                    new Offset(0, 1)
                };

                return BeamAttack(player, beam, MagicEffectType.MortArea, GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10) );
            } },

            { "exori flam", player =>
            {
                var beam = new Offset[] 
                {
                    new Offset(0, 1)
                };

                return BeamAttack(player, beam, MagicEffectType.FireArea, GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10) );
            } },

            { "exori vis", player =>
            {
                var beam = new Offset[] 
                {
                    new Offset(0, 1)
                };

                return BeamAttack(player, beam, MagicEffectType.EnergyArea, GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10) );
            } },

            { "exevo flam hur", player =>
            {
                var beam = new Offset[]
                {
                                                          new Offset(0, 1),
                                       new Offset(-1, 2), new Offset(0, 2), new Offset(1, 2),
                                       new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                    new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4)
                };

                return BeamAttack(player, beam, MagicEffectType.FireArea, GenericFormula(player.Level, player.Skills.MagicLevel, 30, 10) );
            } },

            { "exevo vis lux", player =>
            {
                var beam = new Offset[]
                {
                    new Offset(0, 1),
                    new Offset(0, 2),
                    new Offset(0, 3),
                    new Offset(0, 4),
                    new Offset(0, 5)
                };

                return BeamAttack(player, beam, MagicEffectType.EnergyArea, GenericFormula(player.Level, player.Skills.MagicLevel, 60, 20) );
            } },

            { "exevo gran vis lux", player =>
            {
                var beam = new Offset[]
                {
                    new Offset(0, 1),
                    new Offset(0, 2),
                    new Offset(0, 3),
                    new Offset(0, 4),
                    new Offset(0, 5),
                    new Offset(0, 6),
                    new Offset(0, 7)
                };

                return BeamAttack(player, beam, MagicEffectType.EnergyArea, GenericFormula(player.Level, player.Skills.MagicLevel, 120, 80) );
            } },

            { "exevo mort hur", player =>
            {
                var beam = new Offset[]
                {
                                       new Offset(0, 1),
                                       new Offset(0, 2),
                    new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                    new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
                    new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),
                };

                return BeamAttack(player, beam, MagicEffectType.MortArea, GenericFormula(player.Level, player.Skills.MagicLevel, 150, 50) );
            } },

            { "exevo gran mas vis", player =>
            {
                var area = new Offset[]
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

                return AreaAttack(player, area, MagicEffectType.FireArea, GenericFormula(player.Level, player.Skills.MagicLevel, 250, 50) );
            } },

            { "exevo gran mas pox", player =>
            {
                var area = new Offset[]
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

                return AreaAttack(player, area, MagicEffectType.GreenRings, GenericFormula(player.Level, player.Skills.MagicLevel, 200, 50) );
            } },

            { "exori", player =>
            {
                var area = new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0),                     new Offset(1, 0),
                    new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1)
                };

                return AreaAttack(player, area, MagicEffectType.BlackSpark, BerserkFormula(player.Level, player.Skills.Fist, 0) );
            } }
        };

        private static Func<Context, Promise> Light(Player player, byte level, byte color)
        {
            return context =>
            {
                return context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.BlueShimmer) ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateLightCommand(player, new Light(level, color) ) );
                } );
            };           
        }

        private static Func<Context, Promise> Speed(Player player, ushort speed)
        {
            return context =>
            {
                return context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.GreenShimmer) ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateSpeedCommand(player, speed) );
                } );
            };           
        }

        private static Func<Context, Promise> Healing(Player player, (int Min, int Max) formula)
        {
            return context =>
            {
                return context.AddCommand(new CombatTargetedAttackCommand(player, player, null, MagicEffectType.BlueShimmer, target => Server.Random.Next(formula.Min, formula.Max) ) );
            };           
        }

        private static Func<Context, Promise> Healing(Player player, Offset[] area, (int Min, int Max) formula)
        {
            return context =>
            {
                return context.AddCommand(new CombatAreaAttackCommand(player, player.Tile.Position, area, null, MagicEffectType.BlueShimmer, target => Server.Random.Next(formula.Min, formula.Max) ) );
            };           
        }

        private static Func<Context, Promise> AreaAttack(Player player, Offset[] area, MagicEffectType? magicEffectType, (int Min, int Max) formula)
        {
            return context =>
            {
                return context.AddCommand(new CombatAreaAttackCommand(player, player.Tile.Position, area, null, magicEffectType, target => -Server.Random.Next(formula.Min, formula.Max) ) );
            };
        }

        private static Func<Context, Promise> BeamAttack(Player player, Offset[] beam, MagicEffectType? magicEffectType, (int Min, int Max) formula)
        {
            return context =>
            {
                return context.AddCommand(new CombatBeamAttackCommand(player, beam, magicEffectType, target => -Server.Random.Next(formula.Min, formula.Max) ) );
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
            Func<Player, Func<Context, Promise>> callback;

            if (spells.TryGetValue(command.Message, out callback) )
            {
                return next(context).Then( callback(command.Player) );
            }

            return next(context);
        }
    }
}