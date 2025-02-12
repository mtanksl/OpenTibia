using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class EternalWinterMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public EternalWinterMonsterAttackPlugin() : base(Offset.Circle11, MagicEffectType.IceTornado, DamageType.Ice)
        {

        }
    }
}