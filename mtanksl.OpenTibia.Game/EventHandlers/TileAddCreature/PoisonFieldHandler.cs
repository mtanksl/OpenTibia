using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PoisonFieldHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> poisonFields = new HashSet<ushort>() { 1490, 1496 };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                if (poisonFields.Contains(topItem.Metadata.OpenTibiaId) )
                {
                    context.AddCommand(new CombatConditionCommand(null, e.Creature, SpecialCondition.Poisoned, MagicEffectType.GreenRings, new[] { -5, -5, -5, -5, -5, -4, -4, -4, -4, -4, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }, 2000) );

                    break;
                }
            }
        }
    }
}