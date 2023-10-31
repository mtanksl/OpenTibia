﻿using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;

namespace mtanksl.OpenTibia.GameData.Plugins.Weapons
{
    public class WandOfDragonbreathWeaponPlugin : WeaponPlugin
    {
        public WandOfDragonbreathWeaponPlugin(Weapon weapon) : base(weapon)
        {

        }

        public override void Start()
        {
            
        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
           var formula = WandFormula(19, 6);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DistanceAttack(weapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
        }

        public override void Stop()
        {
           
        }
    }
}