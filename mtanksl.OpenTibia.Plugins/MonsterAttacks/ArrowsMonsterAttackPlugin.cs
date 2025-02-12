using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class ArrowsMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public ArrowsMonsterAttackPlugin() : base(ProjectileType.Arrow, null, DamageType.Physical)
        {

        }
    }
}