using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class HeavyMagicMissileMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public HeavyMagicMissileMonsterAttackPlugin() : base(ProjectileType.EnergySmall, null, DamageType.Energy)
        {

        }
    }
}