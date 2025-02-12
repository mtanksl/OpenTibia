using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class FierceBerserkMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public FierceBerserkMonsterAttackPlugin() : base(Offset.Square3, MagicEffectType.BlackSpark, DamageType.Physical)
        {

        }
    }
}