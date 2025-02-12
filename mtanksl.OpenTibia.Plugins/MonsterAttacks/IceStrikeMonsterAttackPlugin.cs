using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class IceStrikeMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public IceStrikeMonsterAttackPlugin() : base(ProjectileType.IceSmall, MagicEffectType.IceDamage, DamageType.Ice)
        {

        }
    }
}