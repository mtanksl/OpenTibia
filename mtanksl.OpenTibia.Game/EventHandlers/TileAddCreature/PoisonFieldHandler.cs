using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PoisonFieldHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> poisonFields = new HashSet<ushort>() { 1490, 1496 };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                if (poisonFields.Contains(topItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, 

                        new SimpleAttack(null, MagicEffectType.GreenRings, AnimatedTextColor.Green, 5, 5),
                                                                                                                         
                        new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) );
                }
            }

            return Promise.Completed;
        }
    }
}