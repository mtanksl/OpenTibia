using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpellHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerSayCommand command)
        {
            Action<Context> callback = null;

            if (command.Message == "utevo lux")
            {
                callback = Light(command.Player, 6, 215);
            }
            else if (command.Message == "utevo gran lux")
            {
                callback = Light(command.Player, 8, 215);
            }
            else if (command.Message == "utevo vis lux")
            {
                callback = Light(command.Player, 10, 215);
            }
            else if (command.Message == "utani hur")
            {
                callback = Speed(command.Player, (ushort)(command.Player.BaseSpeed * 1.3 - 24) );
            }
            else if (command.Message == "utani gran hur")
            {
                callback = Speed(command.Player, (ushort)(command.Player.BaseSpeed * 1.7 - 56) );
            }
            else if (command.Message == "exura")
            {
                var formula = LightHealingFormula(command.Player.Level, command.Player.Skills.MagicLevel);

                callback = Healing(command.Player, formula.Min, formula.Max);
            }
            else if (command.Message == "exura gran")
            {
                var formula = IntenseHealingFormula(command.Player.Level, command.Player.Skills.MagicLevel);

                callback = Healing(command.Player, formula.Min, formula.Max);
            }
            else if (command.Message == "exura vita")
            {
                var formula = UltimateHealingFormula(command.Player.Level, command.Player.Skills.MagicLevel);

                callback = Healing(command.Player, formula.Min, formula.Max);
            }
            else if (command.Message == "exura gran mas res")
            {
                var formula = MassHealingFormula(command.Player.Level, command.Player.Skills.MagicLevel);

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

                callback = HealingArea(command.Player, area, formula.Min, formula.Max);
            }
            else if (command.Message == "exori mort")
            {
                var formula = Strike(command.Player.Level, command.Player.Skills.MagicLevel);

                var beam = new Offset[] 
                {
                    new Offset(0, 1)
                };

                callback = BeamAttack(command.Player, beam, MagicEffectType.MortArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exori flam")
            {
                var formula = Strike(command.Player.Level, command.Player.Skills.MagicLevel);

                var beam = new Offset[]
                {
                    new Offset(0, 1)
                };

                callback = BeamAttack(command.Player, beam, MagicEffectType.FireArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exori vis")
            {
                var formula = Strike(command.Player.Level, command.Player.Skills.MagicLevel);

                var beam = new Offset[] 
                {
                    new Offset(0, 1)
                };

                callback = BeamAttack(command.Player, beam, MagicEffectType.EnergyArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exevo flam hur")
            {
                var formula = Strike(command.Player.Level, command.Player.Skills.MagicLevel);

                var beam = new Offset[]
                {
                                                          new Offset(0, 1),
                                       new Offset(-1, 2), new Offset(0, 2), new Offset(1, 2),
                                       new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                    new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4)
                };

                callback = BeamAttack(command.Player, beam, MagicEffectType.FireArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exevo vis lux")
            {
                var formula = Strike(command.Player.Level, command.Player.Skills.MagicLevel);

                var beam = new Offset[]
                {
                    new Offset(0, 1),
                    new Offset(0, 2),
                    new Offset(0, 3),
                    new Offset(0, 4),
                    new Offset(0, 5)
                };

                callback = BeamAttack(command.Player, beam, MagicEffectType.EnergyArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exevo gran vis lux")
            {
                var formula = Strike(command.Player.Level, command.Player.Skills.MagicLevel);

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

                callback = BeamAttack(command.Player, beam, MagicEffectType.EnergyArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exevo mort hur")
            {
                var formula = Strike(command.Player.Level, command.Player.Skills.MagicLevel);

                var beam = new Offset[]
                {
                                       new Offset(0, 1),
                                       new Offset(0, 2),
                    new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                    new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
                    new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),
                };

                callback = BeamAttack(command.Player, beam, MagicEffectType.MortArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exevo gran mas vis")
            {
                var formula = Berserk(command.Player.Level, command.Player.Skills.Fist, 0);

                var area = new Offset[]
                {
                                                                                                                       new Offset(0, -5),
                                                                               new Offset(-2, -4), new Offset(-1, -4), new Offset(0, -4), new Offset(1, -4), new Offset(2, -4),
                                                           new Offset(-3, -3), new Offset(-2, -3), new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3), new Offset(2, -3), new Offset(3, -3),
                                       new Offset(-4, -2), new Offset(-3, -2), new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2), new Offset(3, -2), new Offset(4, -2),
                                       new Offset(-4, -1), new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1), new Offset(4, -1),
                    new Offset(-5, 0), new Offset(-4, 0),  new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),                     new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),  new Offset(4, 0),  new Offset(5, 0),
                                       new Offset(-4, 1),  new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),  new Offset(4, 1),
                                       new Offset(-4, 2),  new Offset(-3, 2),  new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),  new Offset(3, 2),  new Offset(4, 2),
                                                           new Offset(-3, 3),  new Offset(-2, 3),  new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3),  new Offset(2, 3),  new Offset(3, 3),
                                                                               new Offset(-2, 4),  new Offset(-1, 4),  new Offset(0, 4),  new Offset(1, 4),  new Offset(2, 4),
                                                                                                                       new Offset(0, 5),
                };

                callback = AreaAttack(command.Player, area, MagicEffectType.FireArea, formula.Min, formula.Max);
            }
            else if (command.Message == "exevo gran mas pox")
            {
                var formula = Berserk(command.Player.Level, command.Player.Skills.Fist, 0);

                var area = new Offset[]
                {
                                                                                                                       new Offset(0, -5),
                                                                               new Offset(-2, -4), new Offset(-1, -4), new Offset(0, -4), new Offset(1, -4), new Offset(2, -4),
                                                           new Offset(-3, -3), new Offset(-2, -3), new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3), new Offset(2, -3), new Offset(3, -3),
                                       new Offset(-4, -2), new Offset(-3, -2), new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2), new Offset(3, -2), new Offset(4, -2),
                                       new Offset(-4, -1), new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1), new Offset(4, -1),
                    new Offset(-5, 0), new Offset(-4, 0),  new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),                     new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),  new Offset(4, 0),  new Offset(5, 0),
                                       new Offset(-4, 1),  new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),  new Offset(4, 1),
                                       new Offset(-4, 2),  new Offset(-3, 2),  new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),  new Offset(3, 2),  new Offset(4, 2),
                                                           new Offset(-3, 3),  new Offset(-2, 3),  new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3),  new Offset(2, 3),  new Offset(3, 3),
                                                                               new Offset(-2, 4),  new Offset(-1, 4),  new Offset(0, 4),  new Offset(1, 4),  new Offset(2, 4),
                                                                                                                       new Offset(0, 5),
                };

                callback = AreaAttack(command.Player, area, MagicEffectType.GreenRings, formula.Min, formula.Max);
            }
            else if (command.Message == "exori")
            {
                var formula = Berserk(command.Player.Level, command.Player.Skills.Fist, 0);

                var area = new Offset[] 
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0),                     new Offset(1, 0),
                    new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1) 
                };

                callback = AreaAttack(command.Player, area, MagicEffectType.BlackSpark, formula.Min, formula.Max);
            }
            
            if (callback != null)
            {
                return next(context).Then(callback);
            }

            return next(context);
        }

        private Action<Context> Light(Player player, byte level, byte color)
        {
            return context =>
            {
                context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.BlueShimmer) );

                context.AddCommand(new CreatureUpdateLightCommand(player, new Light(level, color) ) );
            };           
        }

        private Action<Context> Speed(Player player, ushort speed)
        {
            return context =>
            {
                context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.GreenShimmer) );

                context.AddCommand(new CreatureUpdateSpeedCommand(player, speed) );
            };           
        }

        private Action<Context> Healing(Player player, int min, int max)
        {
            return context =>
            {
                context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.BlueShimmer) );

                context.AddCommand(new CombatDamageCommand(player, player, Server.Random.Next(min, max) ) );
            };           
        }

        private Action<Context> HealingArea(Player player, Offset[] area, int min, int max)
        {
            return context =>
            {
                context.AddCommand(new CombatAreaAttackCommand(player, area, MagicEffectType.BlueShimmer, Server.Random.Next(min, max) ) );
            };           
        }

        private Action<Context> AreaAttack(Player player, Offset[] area, MagicEffectType magicEffectType, int min, int max)
        {
            return context =>
            {
                context.AddCommand(new CombatAreaAttackCommand(player, area, magicEffectType, -Server.Random.Next(min, max) ) );
            };
        }

        private Action<Context> BeamAttack(Player player, Offset[] beam, MagicEffectType magicEffectType, int min, int max)
        {
            return context =>
            {
                context.AddCommand(new CombatBeamAttackCommand(player, beam, magicEffectType, -Server.Random.Next(min, max) ) );
            };
        }

        private (int Min, int Max) LightHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 1.4 + 8), 
                     (int)(level * 0.2 + magicLevel * 1.795 + 11) );
        }

        private (int Min, int Max) IntenseHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 3.184 + 20), 
                     (int)(level * 0.2 + magicLevel * 5.59 + 35) );
        }

        private (int Min, int Max) MassHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 5.7 + 26),
                     (int)(level * 0.2 + magicLevel * 10.43 + 62) );
        }

        private (int Min, int Max) UltimateHealingFormula(int level, int magicLevel)
        {
            return ( (int)(level * 0.2 + magicLevel * 7.22 + 44),
                     (int)(level * 0.2 + magicLevel * 12.79 + 79) );
        }

        private (int Min, int Max) Strike(int level, int magicLevel)
        {
           return ( (int)(level * 0.2 + magicLevel * 1.403 + 8),
                    (int)(level * 0.2 + magicLevel * 2.203 + 13) );
        }

        private (int Min, int Max) Berserk(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon) * 0.5 + level * 0.2),
                     (int)( (skill + weapon) * 1.5 + level * 0.2) );
        }
    }
}