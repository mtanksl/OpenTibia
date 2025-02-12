using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class TerraWaveMonsterAttackPlugin : BaseSpellBeamMonsterAttackPlugin
    {
        public TerraWaveMonsterAttackPlugin() : base(Offset.Wave11333, MagicEffectType.PlantAttack, DamageType.Earth)
        {

        }
    }
}