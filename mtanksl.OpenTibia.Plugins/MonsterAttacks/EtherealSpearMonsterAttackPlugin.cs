using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class EtherealSpearMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public EtherealSpearMonsterAttackPlugin() : base(ProjectileType.EtherenalSpear, MagicEffectType.GroundShaker, DamageType.Physical)
        {

        }
    }
}