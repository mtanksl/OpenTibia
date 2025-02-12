using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class HolyMissileMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public HolyMissileMonsterAttackPlugin() : base(ProjectileType.Holy, null, DamageType.Holy)
        {

        }
    }
}