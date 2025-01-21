using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class EnergyFieldHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly HashSet<ushort> energyFields;

        public EnergyFieldHandler()
        {
            energyFields = Context.Server.Values.GetUInt16HashSet("values.items.energyFields");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    if (energyFields.Contains(topItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, 

                            new SimpleAttack(null, null, DamageType.Energy, 30, 30),
                                                                                                                         
                            new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) ) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}