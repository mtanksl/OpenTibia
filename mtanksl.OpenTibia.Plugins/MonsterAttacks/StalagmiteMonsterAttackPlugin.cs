using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class StalagmiteMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public StalagmiteMonsterAttackPlugin() : base(ProjectileType.Poison, null, DamageType.Earth)
        {

        }
    }
}