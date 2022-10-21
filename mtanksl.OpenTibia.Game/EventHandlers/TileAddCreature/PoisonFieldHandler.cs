using OpenTibia.Game.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
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
                    context.AddCommand(new CombatConditionCommand(e.Creature, MagicEffectType.GreenRings, new[] { -5, -5, -5 }, new[] { 2000, 2000, 2000 } ) );

                    break;
                }
            }
        }
    }
}