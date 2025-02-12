using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class RageOfTheSkiesMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public RageOfTheSkiesMonsterAttackPlugin() : base(Offset.Circle11, MagicEffectType.BigClouds, DamageType.Energy)
        {

        }
    }
}