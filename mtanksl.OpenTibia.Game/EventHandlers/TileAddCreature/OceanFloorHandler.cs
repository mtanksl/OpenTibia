using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class OceanFloorHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> oceanFloors = new HashSet<ushort>() { 5405, 5406, 5407, 5408, 5409, 5410 };

        private ushort helmetOfTheDeep = 5461;

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.Creature is Player player && e.Tile.Ground != null)
            {
                if (oceanFloors.Contains(e.Tile.Ground.Metadata.OpenTibiaId) )
                {
                    if ( !player.HasSpecialCondition(SpecialCondition.Drowning) )
                    {
                        Item item = player.Inventory.GetContent( (int)Slot.Head) as Item;

                        if (item == null || item.Metadata.OpenTibiaId != helmetOfTheDeep)
                        {
                            return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, 

                                new SimpleAttack(null, MagicEffectType.BlueRings, AnimatedTextColor.Crystal, 20, 20),

                                new DrowningCondition(20, TimeSpan.FromSeconds(4) ) ) );
                        }
                    }
                }
                else
                {
                    if (player.HasSpecialCondition(SpecialCondition.Drowning) )
                    {
                        return Context.AddCommand(new CreatureRemoveConditionCommand(e.Creature, ConditionSpecialCondition.Drowning) );
                    }
                }
            }            

            return Promise.Completed;
        }
    }
}