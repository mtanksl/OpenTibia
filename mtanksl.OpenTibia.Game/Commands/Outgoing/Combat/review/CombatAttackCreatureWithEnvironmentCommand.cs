using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureWithEnvironmentCommand : Command
    {
        public CombatAttackCreatureWithEnvironmentCommand(Creature target, MagicEffectType damageMagicEffectType, int damage)
        {
            Target = target;

            DamageMagicEffectType = damageMagicEffectType;

            Damage = damage;
        }

        public Creature Target { get; set; }

        public MagicEffectType DamageMagicEffectType { get; set; }

        public int Damage { get; set; }

        public override Promise Execute()
        {
            CombatAttackCreatureBuilder builder = new CombatAttackCreatureBuilder()
            {
                Attacker = null,
                Target = Target,
                ProjectileType = null,
                MagicEffectType = null,
                Formula = new DamageDto()
                {
                    Formula = (attacker, target) => Damage,
                    MissedMagicEffectType = null,
                    DamageMagicEffectType = DamageMagicEffectType,
                    DamageAnimatedTextColor = AnimatedTextColor.DarkRed
                },
                Condition = null
            };

            return builder.Build();
        }
    }
}