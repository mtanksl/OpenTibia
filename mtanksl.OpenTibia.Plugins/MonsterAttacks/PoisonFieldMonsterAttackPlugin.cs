using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class PoisonFieldMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public PoisonFieldMonsterAttackPlugin() : base(Offset.Square1, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, DamageType.Earth, new DamageCondition(SpecialCondition.Poisoned, null, DamageType.Earth, new[] { 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) )
        {

        }
    }
}