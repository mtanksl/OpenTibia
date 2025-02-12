using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class ThunderstormMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public ThunderstormMonsterAttackPlugin() : base(Offset.Circle7, ProjectileType.Energy, MagicEffectType.EnergyDamage, DamageType.Energy)
        {

        }
    }
}