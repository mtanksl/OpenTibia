using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpellHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerSayCommand command)
        {
            if (command.Message == "utevo lux")
            {
                return next(context).Then(ctx =>
                { 
                    return ctx.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.BlueShimmer) );

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateLightCommand(command.Player, new Light(6, 215) ) );
                } );
            }
            else if (command.Message == "utevo gran lux")
            {
                return next(context).Then(ctx =>
                { 
                    return ctx.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.BlueShimmer) );

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateLightCommand(command.Player, new Light(8, 215) ) );
                } );
            }
            else if (command.Message == "utevo vis lux")
            {
                return next(context).Then(ctx =>
                { 
                    return ctx.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.BlueShimmer) );

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new CreatureUpdateLightCommand(command.Player, new Light(10, 215) ) );
                } );
            }
            else if (command.Message == "exura")
            {
                return next(context).Then(ctx =>
                { 
                    return ctx.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.BlueShimmer) );

                } ).Then(ctx =>
                {
                    var formula = LightHealingFormula(command.Player.Level, command.Player.Skills.MagicLevel);

                    int health = Server.Random.Next(formula.Item1, formula.Item2);

                    if (command.Player.Health + health > command.Player.MaxHealth)
                    {
                        return ctx.AddCommand(new CreatureUpdateHealthCommand(command.Player, command.Player.MaxHealth, command.Player.MaxHealth) );
                    }

                    return ctx.AddCommand(new CreatureUpdateHealthCommand(command.Player, (ushort)(command.Player.Health + health), command.Player.MaxHealth) );
                } );
            }
            else if (command.Message == "exura gran")
            {
                return next(context).Then(ctx =>
                { 
                    return ctx.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.BlueShimmer) );

                } ).Then(ctx =>
                {
                    var formula = IntenseHealingFormula(command.Player.Level, command.Player.Skills.MagicLevel);

                    int health = Server.Random.Next(formula.Item1, formula.Item2);

                    if (command.Player.Health + health > command.Player.MaxHealth)
                    {
                        return ctx.AddCommand(new CreatureUpdateHealthCommand(command.Player, command.Player.MaxHealth, command.Player.MaxHealth) );
                    }

                    return ctx.AddCommand(new CreatureUpdateHealthCommand(command.Player, (ushort)(command.Player.Health + health), command.Player.MaxHealth) );
                } );
            }
            else if (command.Message == "exura vita")
            {
                return next(context).Then(ctx =>
                { 
                    return ctx.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.BlueShimmer) );

                } ).Then(ctx =>
                {
                    var formula = UltimateHealingFormula(command.Player.Level, command.Player.Skills.MagicLevel);

                    int health = Server.Random.Next(formula.Item1, formula.Item2);

                    if (command.Player.Health + health > command.Player.MaxHealth)
                    {
                        return ctx.AddCommand(new CreatureUpdateHealthCommand(command.Player, command.Player.MaxHealth, command.Player.MaxHealth) );
                    }

                    return ctx.AddCommand(new CreatureUpdateHealthCommand(command.Player, (ushort)(command.Player.Health + health), command.Player.MaxHealth) );
                } );
            }
            else
            {
                //TODO: Spells
            }

            return next(context);
        }

        private Tuple<int, int> LightHealingFormula(int level, int magicLevel)
        {
            return Tuple.Create( (int)( (level * 0.2) + (magicLevel * 1.4) + 8), 
                                 (int)( (level * 0.2) + (magicLevel * 1.795) + 11) );
        }

        private Tuple<int, int> IntenseHealingFormula(int level, int magicLevel)
        {
            return Tuple.Create( (int)( (level * 0.2) + (magicLevel * 3.184) + 20), 
                                 (int)( (level * 0.2) + (magicLevel * 5.59) + 35) );
        }

        private Tuple<int, int> UltimateHealingFormula(int level, int magicLevel)
        {
            return Tuple.Create( (int)( (level * 0.2) + (magicLevel * 7.22) + 44),
                                 (int)( (level * 0.2) + (magicLevel * 12.79) + 79) );
        }
    }
}