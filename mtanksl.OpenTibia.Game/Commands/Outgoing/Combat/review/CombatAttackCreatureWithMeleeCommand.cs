using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureWithMeleeCommand : Command
    {
        public CombatAttackCreatureWithMeleeCommand(Creature attacker, Creature target, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Target = target;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute()
        {
            CombatAttackCreatureBuilder builder = new CombatAttackCreatureBuilder()
            {
                Attacker = Attacker,
                Target = Target,
                ProjectileType = null,
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