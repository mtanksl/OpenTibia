using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class GroundshakerMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public GroundshakerMonsterAttackPlugin() : base(Offset.Circle7, MagicEffectType.GroundShaker, DamageType.Physical)
        {

        }
    }
}