using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class LifeDrainMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public LifeDrainMonsterAttackPlugin() : base(null, null, DamageType.LifeDrain)
        {

        }
    }
}