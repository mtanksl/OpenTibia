using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class GreatEnergyBeamMonsterAttackPlugin : BaseSpellBeamMonsterAttackPlugin
    {
        public GreatEnergyBeamMonsterAttackPlugin() : base(Offset.Beam7, MagicEffectType.EnergyArea, DamageType.Energy)
        {

        }
    }
}