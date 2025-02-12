using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class LightMagicMissileMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public LightMagicMissileMonsterAttackPlugin() : base(ProjectileType.EnergySmall, null, DamageType.Energy)
        {

        }
    }
}