using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class BerserkMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public BerserkMonsterAttackPlugin() : base(Offset.Square3, MagicEffectType.BlackSpark, DamageType.Physical)
        {

        }
    }
}