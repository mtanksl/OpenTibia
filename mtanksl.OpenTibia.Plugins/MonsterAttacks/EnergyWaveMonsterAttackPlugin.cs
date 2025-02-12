using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class EnergyWaveMonsterAttackPlugin : BaseSpellBeamMonsterAttackPlugin
    {
        public EnergyWaveMonsterAttackPlugin() : base(Offset.Wave11333, MagicEffectType.EnergyArea, DamageType.Energy)
        {

        }
    }
}