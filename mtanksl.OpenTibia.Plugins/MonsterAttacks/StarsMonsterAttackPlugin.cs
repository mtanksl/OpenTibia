using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class StarsMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public StarsMonsterAttackPlugin() : base(ProjectileType.ThrowingStar, null, DamageType.Physical)
        {

        }
    }
}