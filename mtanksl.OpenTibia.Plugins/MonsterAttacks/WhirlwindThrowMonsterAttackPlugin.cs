using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class WhirlwindThrowMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public WhirlwindThrowMonsterAttackPlugin() : base(ProjectileType.WhirlWindSword, MagicEffectType.GroundShaker, DamageType.Physical)
        {

        }
    }
}