using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureBuilder
    {
        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public DamageDto Formula { get; set; }

        public ConditionDto Condition { get; set; }

        public async Promise Build()
        {
            if (ProjectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Target.Tile.Position, ProjectileType.Value) );
            }

            if (MagicEffectType != null)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MagicEffectType.Value) );
            }
              
            if (Formula != null)
            {
                await Context.Current.AddCommand(new CombatAddDamageCommand(Attacker, Target, Formula.Formula, Formula.MissedMagicEffectType, Formula.DamageMagicEffectType, Formula.DamageAnimatedTextColor) );

                if (Target.Health == 0)
                {
                    return;
                }
            }  

            // if (Condition != null)
            // {
            //     _ = Context.Current.AddCommand(new CombatAddConditionCommand(Target, Condition.SpecialCondition, Condition.MagicEffectType, Condition.AnimatedTextColor, Condition.Damages, Condition.IntervalInMilliseconds) );
            // 
            //     if (Target.Health == 0)
            //     {
            //         return;
            //     }
            // }                   
        }
    }
}