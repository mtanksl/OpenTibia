using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class OceanFloorHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly HashSet<ushort> oceanFloors;
        private readonly HashSet<ushort> helmetOfTheDeeps;

        public OceanFloorHandler()
        {
            oceanFloors = Context.Server.Values.GetUInt16HashSet("values.items.oceanFloors");
            helmetOfTheDeeps = Context.Server.Values.GetUInt16HashSet("values.items.helmetOfTheDeeps");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.Creature is Player player && e.ToTile.Ground != null)
            {
                if (oceanFloors.Contains(e.ToTile.Ground.Metadata.OpenTibiaId) )
                {
                    if ( !player.HasSpecialCondition(SpecialCondition.Drowning) )
                    {
                        if ( !(player.Inventory.GetContent( (int)Slot.Head) is Item item) || !helmetOfTheDeeps.Contains(item.Metadata.OpenTibiaId) )
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