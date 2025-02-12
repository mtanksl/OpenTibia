using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class DivineCalderaMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public DivineCalderaMonsterAttackPlugin() : base(Offset.Circle7, MagicEffectType.HolyDamage, DamageType.Holy)
        {

        }
    }
}