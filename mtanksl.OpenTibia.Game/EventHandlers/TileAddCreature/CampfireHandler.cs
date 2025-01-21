using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CampfireHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly HashSet<ushort> campfires;

        public CampfireHandler()
        {
            campfires = Context.Server.Values.GetUInt16HashSet("values.items.campfires");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    if (campfires.Contains(topItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, 

                            new SimpleAttack(null, null, DamageType.Fire, 20, 20),
                                                                                                                         
                            new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) ) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}