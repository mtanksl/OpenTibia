using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class BoulderThrowMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public BoulderThrowMonsterAttackPlugin() : base(ProjectileType.BigStone, null, DamageType.Physical)
        {

        }
    }
}