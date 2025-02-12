using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class DivineMissileMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public DivineMissileMonsterAttackPlugin() : base(ProjectileType.HolySmall, MagicEffectType.HolyDamage, DamageType.Holy)
        {

        }
    }
}