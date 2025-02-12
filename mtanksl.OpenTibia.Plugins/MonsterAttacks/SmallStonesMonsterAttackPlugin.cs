using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class SmallStonesMonsterAttackPlugin : BaseDistanceMonsterAttackPlugin
    {
        public SmallStonesMonsterAttackPlugin() : base(ProjectileType.SmallStone, null, DamageType.Physical)
        {

        }
    }
}