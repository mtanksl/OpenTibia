using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class DeathStrikeMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public DeathStrikeMonsterAttackPlugin() : base(ProjectileType.SuddenDeath, MagicEffectType.MortArea, DamageType.Death)
        {

        }
    }
}