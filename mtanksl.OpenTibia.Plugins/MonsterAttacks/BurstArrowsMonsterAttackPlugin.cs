using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class BurstArrowsMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public BurstArrowsMonsterAttackPlugin() : base(Offset.Square3, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire)
        {

        }
    }
}