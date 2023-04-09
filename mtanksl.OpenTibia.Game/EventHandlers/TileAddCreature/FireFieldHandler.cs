using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FireFieldHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> fireFields = new HashSet<ushort>() { 1487, 1488, 1492, 1493 };

        public override void Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                if (fireFields.Contains(topItem.Metadata.OpenTibiaId) )
                {
                    context.AddCommand(new CombatConditionCommand(null, e.Creature, SpecialCondition.Burning, MagicEffectType.FirePlume, new[] { -20, -10, -10, -10, -10, -10, -10, -10 }, 2000) );

                    break;
                }
            }
        }
    }
}