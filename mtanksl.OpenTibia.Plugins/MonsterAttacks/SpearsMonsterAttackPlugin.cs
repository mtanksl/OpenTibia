using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class SpearsMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public SpearsMonsterAttackPlugin() : base(ProjectileType.Spear, null, DamageType.Physical)
        {

        }
    }
}