using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureWithRuneOrSpellCommand : Command
    {
        public CombatAttackCreatureWithRuneOrSpellCommand(Creature attacker, Creature target, ProjectileType? projectileType, MagicEffectType magicEffectType, Func<Creature, Creature, int> formula)
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

        public MagicEffectType MagicEffectType { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute()
        {
            var builder = new CombatAttackCreatureBuilder()
                .WithAttacker(Attacker)
                .WithTarget(Target)
                .WithProjectileType(ProjectileType)
                .WithMagicEffectType(MagicEffectType)
                .WithMissedMagicEffectType(null)
                .WithDamageMagicEffectType(MagicEffectType.RedSpark)
                .WithAnimatedTextColor(AnimatedTextColor.DarkRed)
                .WithFormula(Formula);

            return builder.Build();
        }
    }
}