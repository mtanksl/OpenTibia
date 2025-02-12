using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class WrathOfNatureMonsterAttackPlugin : BaseSpellAreaMonsterAttackPlugin
    {
        public WrathOfNatureMonsterAttackPlugin() : base(Offset.Circle11, MagicEffectType.PlantAttack, DamageType.Earth)
        {

        }
    }
}