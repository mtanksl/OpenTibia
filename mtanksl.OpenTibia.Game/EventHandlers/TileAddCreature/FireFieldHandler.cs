using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FireFieldHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private static HashSet<ushort> fireFields = new HashSet<ushort>() { 1487, 1488, 1492, 1493 };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    if (fireFields.Contains(topItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, 

                            new SimpleAttack(null, MagicEffectType.FirePlume, AnimatedTextColor.Orange, 20, 20),
                                                                                                                         
                            new DamageCondition(SpecialCondition.Burning, MagicEffectType.FirePlume, AnimatedTextColor.Orange, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) ) );
                    }
                }
            }

            return Promise.Completed;            
        }
    }
}