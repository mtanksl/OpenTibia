using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class SoulfireMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public SoulfireMonsterAttackPlugin() : base(null, null, DamageType.Fire, new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(9) ) )
        {

        }
    }
}