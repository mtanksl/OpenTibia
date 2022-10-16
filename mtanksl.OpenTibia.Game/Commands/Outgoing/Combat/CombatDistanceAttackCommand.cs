using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CombatDistanceAttackCommand : Command
    {
        public CombatDistanceAttackCommand(Creature attacker, Creature target, ProjectileType projectileType, MagicEffectType? magicEffectType, int health)
        {
            Attacker = attacker;

            Target = target;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            Health = health;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public int Health { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Target.Tile.Position, ProjectileType) );

                if (MagicEffectType != null)
                {
                    context.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MagicEffectType.Value) );
                }

                context.AddCommand(new CombatChangeHealthCommand(Attacker, Target, Health) );

                resolve(context);
            } );
        }
    }
}