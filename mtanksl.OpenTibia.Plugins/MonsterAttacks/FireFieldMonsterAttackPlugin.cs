using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class FireFieldMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public FireFieldMonsterAttackPlugin() : base(Offset.Square1, ProjectileType.Fire, MagicEffectType.FireDamage, 1492, 1, DamageType.Fire, new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) )
        {

        }
    }  
}