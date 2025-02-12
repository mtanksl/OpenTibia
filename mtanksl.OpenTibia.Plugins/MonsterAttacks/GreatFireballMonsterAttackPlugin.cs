using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class GreatFireballMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public GreatFireballMonsterAttackPlugin() : base(Offset.Circle7, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire)
        {

        }
    }
}