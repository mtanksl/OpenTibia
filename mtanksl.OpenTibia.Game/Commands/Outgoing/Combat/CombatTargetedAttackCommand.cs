using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatTargetedAttackCommand : Command
    {
        public CombatTargetedAttackCommand(Creature attacker, Creature target, ProjectileType? projectileType, MagicEffectType? magicEffectType, Func<Creature, int> formula)
        {
            Attacker = attacker;

            Target = target;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public Func<Creature, int> Formula { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (ProjectileType != null)
                {
                    context.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Target.Tile.Position, ProjectileType.Value) );
                }

                if (MagicEffectType != null)
                {
                    context.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MagicEffectType.Value) );
                }

                int health = Formula(Target);

                if (Target != Attacker || health > 0)
                {
                    context.AddCommand(new CombatChangeHealthCommand(Attacker, Target, health) );
                }

                resolve(context);
            } );
        }
    }
}