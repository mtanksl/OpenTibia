using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class EnergyBeamMonsterAttackPlugin : BaseSpellBeamMonsterAttackPlugin
    {
        public EnergyBeamMonsterAttackPlugin() : base(Offset.Beam5, MagicEffectType.EnergyArea, DamageType.Energy)
        {

        }
    }
}