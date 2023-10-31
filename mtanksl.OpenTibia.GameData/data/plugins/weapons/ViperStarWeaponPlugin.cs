﻿using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace mtanksl.OpenTibia.GameData.Plugins.Weapons
{
    public class ViperStarWeaponPlugin : WeaponPlugin
    {
        public ViperStarWeaponPlugin(Weapon weapon) : base(weapon)
        {

        }

        public override void Start()
        {
            
        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
             var formula = DistanceFormula(player.Level, player.Skills.Distance, weapon.Metadata.Attack.Value, player.Client.FightMode);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DistanceAttack(weapon.Metadata.ProjectileType.Value, formula.Min, formula.Max),

                new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 2, 2, 2, 2, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) );
        }

        public override void Stop()
        {
           
        }
    }
}