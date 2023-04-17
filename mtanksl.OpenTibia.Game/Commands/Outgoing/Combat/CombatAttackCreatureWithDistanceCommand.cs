using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureWithDistanceCommand : Command
    {
        public CombatAttackCreatureWithDistanceCommand(Creature attacker, Creature target, ProjectileType projectileType, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Target = target;

            ProjectileType = projectileType;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute()
        {
            CombatAttackCreatureBuilder builder = new CombatAttackCreatureBuilder()
            {
                Attacker = Attacker,
                Target = Target,
                ProjectileType = ProjectileType,
                MagicEffectType = null,
                Formula = new DamageDto()
                {
                    Formula = Formula,
                    MissedMagicEffectType = MagicEffectType.Puff,
                    DamageMagicEffectType = MagicEffectType.RedSpark,
                    DamageAnimatedTextColor = AnimatedTextColor.DarkRed
                },
                Condition = null
            };

            return builder.Build();
        }
    }
}