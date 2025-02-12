using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class SuddenDeathMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public SuddenDeathMonsterAttackPlugin() : base(ProjectileType.SuddenDeath, MagicEffectType.MortArea, DamageType.Death)
        {

        }
    }
}