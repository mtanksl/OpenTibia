using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class EnergyBombMonsterAttackPlugin : BaseRuneAreaMonsterAttackPlugin
    {
        public EnergyBombMonsterAttackPlugin() : base(Offset.Square3, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, DamageType.Energy, new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) )
        {

        }
    }
}