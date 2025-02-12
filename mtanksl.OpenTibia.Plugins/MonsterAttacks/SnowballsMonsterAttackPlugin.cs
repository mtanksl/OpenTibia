using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class SnowballsMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public SnowballsMonsterAttackPlugin() : base(ProjectileType.Snowball, null, DamageType.Physical)
        {

        }
    }
}