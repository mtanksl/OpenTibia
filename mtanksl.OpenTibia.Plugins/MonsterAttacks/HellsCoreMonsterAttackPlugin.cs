using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class HellsCoreMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public HellsCoreMonsterAttackPlugin() : base(Offset.Circle11, MagicEffectType.FireArea, DamageType.Fire)
        {

        }
    }
}