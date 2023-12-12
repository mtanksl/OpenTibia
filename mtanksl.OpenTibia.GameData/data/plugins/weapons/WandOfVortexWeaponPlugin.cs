﻿using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;

namespace OpenTibia.GameData.Plugins.Weapons
{
    public class WandOfVortexWeaponPlugin : WeaponPlugin
    {
        public WandOfVortexWeaponPlugin(Weapon weapon) : base(weapon)
        {

        }

        public override void Start()
        {
            
        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
           var formula = WandFormula(13, 5);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DistanceAttack(weapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
        }

        public override void Stop()
        {
           
        }
    }
}