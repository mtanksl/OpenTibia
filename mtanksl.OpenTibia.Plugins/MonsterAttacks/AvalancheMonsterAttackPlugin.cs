using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class AvalancheMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public AvalancheMonsterAttackPlugin() : base(Offset.Circle7, ProjectileType.Ice, MagicEffectType.IceArea, DamageType.Ice)
        {

        }
    }
}