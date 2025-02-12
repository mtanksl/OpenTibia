using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class EnergyFieldMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public EnergyFieldMonsterAttackPlugin() : base(Offset.Square1, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, DamageType.Energy, new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) )
        {

        }
    }
}