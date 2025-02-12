using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class KnivesMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public KnivesMonsterAttackPlugin() : base(ProjectileType.ThrowingKnife, null, DamageType.Physical)
        {

        }
    }
}