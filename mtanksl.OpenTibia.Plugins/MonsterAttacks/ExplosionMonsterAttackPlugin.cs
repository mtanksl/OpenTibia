using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class ExplosionMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public ExplosionMonsterAttackPlugin() : base(Offset.Circle3, ProjectileType.Explosion, MagicEffectType.ExplosionArea, DamageType.Physical)
        {

        }
    }
}