using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class FlameStrikeMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public FlameStrikeMonsterAttackPlugin() : base(ProjectileType.Fire, MagicEffectType.FireDamage, DamageType.Fire)
        {

        }
    }
}