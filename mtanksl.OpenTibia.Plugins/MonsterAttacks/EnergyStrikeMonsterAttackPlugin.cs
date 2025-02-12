using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class EnergyStrikeMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public EnergyStrikeMonsterAttackPlugin() : base(ProjectileType.EnergySmall, MagicEffectType.EnergyArea, DamageType.Energy)
        {

        }
    }
}