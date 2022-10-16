using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class FarAwayRunesHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private ushort fireball = 2302;

        private ushort greatFireball = 2304;

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (command.ToItem.Parent is Tile tile)
            {
                Action<Context> callback = null;

                if (command.Item.Metadata.OpenTibiaId == fireball)
                {
                    var formula = Generic(command.Player.Level, command.Player.Skills.MagicLevel, 20, 5);

                    var area = new Offset[] 
                    {
                                            new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2),
                        new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1),
                        new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),
                        new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),
                                            new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2) 
                    };

                    callback = AreaAttack(command.Player, command.Item, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, formula.Min, formula.Max);
                }
                else if (command.Item.Metadata.OpenTibiaId == greatFireball)
                {
                    var formula = Generic(command.Player.Level, command.Player.Skills.MagicLevel, 50, 15);

                    var area = new Offset[] 
                    {
                                                                new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
                                            new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
                        new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
                        new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
                        new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
                                            new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
                                                                new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3),
                    };

                    callback = AreaAttack(command.Player, command.Item, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea, formula.Min, formula.Max);
                }

                if (callback != null)
                {
                    return Promise.FromResult(context).Then(callback);
                }
            }

            return next(context);
        }

        private Action<Context> AreaAttack(Player player, Item item, Position position, Offset[] area, ProjectileType projectile, MagicEffectType magicEffectType, int min, int max)
        {
            return context =>
            {
                context.AddCommand(new CombatAreaAttackCommand(player, position, area, projectile, magicEffectType, target => -Server.Random.Next(min, max) ) );

                context.AddCommand(new ItemDecrementCommand(item, 1) );
            };
        }

        private (int Min, int Max) Generic(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        private (int Min, int Max) Generic(int level, int magicLevel, int @base, int variation, int min, int max)
        {
            var formula = 3 * magicLevel + 2 * level;

            if (formula < min)
            {
                formula = min;
            }
            else if (formula > max)
            {
                formula = max;
            }

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }
    }
}