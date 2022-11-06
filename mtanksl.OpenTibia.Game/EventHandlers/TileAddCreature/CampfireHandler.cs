using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CampfireHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> campfires = new HashSet<ushort>() { 1423, 1424, 1425 };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                if (campfires.Contains(topItem.Metadata.OpenTibiaId) )
                {
                    context.AddCommand(new CombatConditionCommand(e.Creature, SpecialCondition.Burning, MagicEffectType.FirePlume, new[] { -20, -10, -10 }, new[] { 2000, 2000 }));

                    break;
                }
            }
        }
    }
}