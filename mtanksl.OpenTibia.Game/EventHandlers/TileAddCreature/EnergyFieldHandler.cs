﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class EnergyFieldHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private static HashSet<ushort> energyFields = new HashSet<ushort>() { 1491, 1495 };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    if (energyFields.Contains(topItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, 

                            new SimpleAttack(null, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, 30, 30),
                                                                                                                         
                            new DamageCondition(SpecialCondition.Electrified, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) ) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}