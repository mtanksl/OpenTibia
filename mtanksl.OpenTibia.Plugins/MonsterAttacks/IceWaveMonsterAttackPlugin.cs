using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class IceWaveMonsterAttackPlugin : BaseSpellBeamMonsterAttackPlugin
    {
        public IceWaveMonsterAttackPlugin() : base(Offset.Wave1335, MagicEffectType.IceArea, DamageType.Ice)
        {

        }
    }
}