using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class FireWaveMonsterAttackPlugin : BaseSpellBeamMonsterAttackPlugin
    {
        public FireWaveMonsterAttackPlugin() : base(Offset.Wave11333, MagicEffectType.FireArea, DamageType.Fire)
        {

        }
    }
}