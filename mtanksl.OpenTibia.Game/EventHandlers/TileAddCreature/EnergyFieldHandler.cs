using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class EnergyFieldHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> energyFields = new HashSet<ushort>() { 1491, 1495 };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                if (energyFields.Contains(topItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new CombatConditionCommand(null, e.Creature, SpecialCondition.Electrified, MagicEffectType.EnergyDamage, new[] { -30, -25, -25 }, 2000) );
                }
            }

            return Promise.Completed;
        }
    }
}