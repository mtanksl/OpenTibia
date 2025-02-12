using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class IcicleMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public IcicleMonsterAttackPlugin() : base(ProjectileType.Ice, MagicEffectType.IceArea, DamageType.Ice)
        {

        }
    }
}