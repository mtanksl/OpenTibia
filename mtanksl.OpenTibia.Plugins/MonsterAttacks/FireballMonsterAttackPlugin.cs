using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class FireballMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public FireballMonsterAttackPlugin() : base(Offset.Circle5, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire)
        {

        }
    }
}