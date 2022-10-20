using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatAreaAttackCommand : Command
    {
        public CombatAreaAttackCommand(Creature attacker, Position position, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Position = position;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Position Position { get; set; }

        public Offset[] Area { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (ProjectileType != null)
                {
                    context.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Position, ProjectileType.Value) );
                }

                foreach (var offset in Area)
                {
                    Position position = Position.Offset(offset);

                    if (MagicEffectType != null)
                    {
                        context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                    }

                    Tile tile = context.Server.Map.GetTile(position);

                    if (tile != null)
                    {
                        foreach (var target in tile.GetMonsters().Concat<Creature>(tile.GetPlayers() ).ToList() )
                        {
                            int health = Formula(Attacker, target);

                            if (target != Attacker || health > 0)
                            {
                                context.AddCommand(new CombatChangeHealthCommand(Attacker, target, health) );
                            }
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}