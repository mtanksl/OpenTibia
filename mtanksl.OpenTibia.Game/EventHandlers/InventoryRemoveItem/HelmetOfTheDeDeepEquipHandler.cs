﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class HelmetOfTheDeepDeEquipHandler : EventHandlers.EventHandler<InventoryRemoveItemEventArgs>
    {
        private static HashSet<ushort> oceanFloors = new HashSet<ushort>() { 5405, 5406, 5407, 5408, 5409, 5410 };

        private static HashSet<ushort> helmetOfTheDeeps = new HashSet<ushort>() { 5461 };

        public override Promise Handle(InventoryRemoveItemEventArgs e)
        {
            if (helmetOfTheDeeps.Contains(e.Item.Metadata.OpenTibiaId) )
            {
                if ( (Slot)e.Slot == Slot.Head)
                {
                    if (oceanFloors.Contains(e.Inventory.Player.Tile.Ground.Metadata.OpenTibiaId) )
                    {
                        if ( !e.Inventory.Player.HasSpecialCondition(SpecialCondition.Drowning) )
                        {
                            return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Inventory.Player, 

                                new SimpleAttack(null, MagicEffectType.BlueRings, AnimatedTextColor.Crystal, 20, 20),

                                new DrowningCondition(20, TimeSpan.FromSeconds(4) ) ) );
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}