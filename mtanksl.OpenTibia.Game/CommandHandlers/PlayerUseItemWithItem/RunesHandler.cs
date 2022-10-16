using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RunesHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static Dictionary<ushort, Func<Player, Position, Func<Context, Promise>>> runes = new Dictionary<ushort, Func<Player, Position, Func<Context, Promise>>>()
        {
            { 2285 /* Poison field */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(0, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, -5);
            } },

            { 2286 /* Poison bomb */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                    new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                };

                return AreaCreate(player, position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, -5);
            } },

            { 2289 /* Poison wall */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, -5);
            } },

            { 2301 /* Fire field */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(0, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, -20);
            } },

            { 2305 /* Fire bomb */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                    new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                };

                return AreaCreate(player, position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, -20);
            } },

            { 2303 /* Fire wall */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, -20);
            } },

            { 2277 /* Energy field */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(0, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, 1495, 1, -30);
            } },

            { 2262 /* Energy bomb */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                    new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
                    new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
                };

                return AreaCreate(player, position, area, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, 1495, 1, -30);
            } },

            { 2279 /* Energy wall */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, 1495, 1, -30);
            } },

            { 2302 /* Fireball */, (player, position) =>
            {
                var area = new Offset[] 
                {
                                        new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2),
                    new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1),
                    new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),
                    new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),
                                        new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2) 
                };

                return AreaAttack(player, position, area, ProjectileType.Fire, MagicEffectType.FireArea, GenericFormula(player.Level, player.Skills.MagicLevel, 20, 5) );
            } },

            { 2304 /* Great fireball */ , (player, position) =>
            {
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

                return AreaAttack(player, position, area, ProjectileType.Fire, MagicEffectType.FireArea, GenericFormula(player.Level, player.Skills.MagicLevel, 50, 15) );
            } },

            { 2313 /* Explosion */, (player, position) =>
            {
                var area = new Offset[]
                {
                                        new Offset(0, -1),
                    new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                                        new Offset(0, 1),
                };

                return AreaAttack(player, position, area, ProjectileType.Explosion, MagicEffectType.ExplosionArea, GenericFormula(player.Level, player.Skills.MagicLevel, 60, 40) );
            } },

            { 2293 /* Magic wall */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(0, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.Energy, null, 1497, 1, 0);
            } },

            { 2269 /* Wild growth */, (player, position) =>
            {
                var area = new Offset[]
                {
                    new Offset(0, 0)
                };

                return AreaCreate(player, position, area, ProjectileType.Earth, null, 1499, 1, 0);
            } }
        };

        private static Func<Context, Promise> AreaAttack(Player player, Position position, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, (int Min, int Max) formula)
        {
            return context =>
            {
                return context.AddCommand(new CombatAreaAttackCommand(player, position, area, projectileType, magicEffectType, target => -Server.Random.Next(formula.Min, formula.Max) ) );
            };
        }

        private static Func<Context, Promise> AreaCreate(Player player, Position position, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, int health)
        {
            return context =>
            {
                return context.AddCommand(new CombatAreaCreateCommand(player, position, area, projectileType, magicEffectType, openTibiaId, count, target => health) );
            };
        }

        private static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithItemCommand command)
        {
            Func<Player, Position, Func<Context, Promise>> callback;

            if (runes.TryGetValue(command.Item.Metadata.OpenTibiaId, out callback) && command.ToItem.Parent is Tile toTile)
            {
                return Promise.FromResult(context).Then( callback(command.Player, toTile.Position) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemDecrementCommand(command.Item, 1) );
                } );
            }

            return next(context);
        }
    }
}