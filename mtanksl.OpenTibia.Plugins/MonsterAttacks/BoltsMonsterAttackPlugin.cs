using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class BoltsMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public BoltsMonsterAttackPlugin() : base(ProjectileType.Bolt, null, DamageType.Physical)
        {

        }
    }
}