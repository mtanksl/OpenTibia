using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class StoneShowerMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public StoneShowerMonsterAttackPlugin() : base(Offset.Circle7, ProjectileType.SmallStone, MagicEffectType.Stones, DamageType.Earth)
        {

        }
    }
}