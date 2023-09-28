using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class OpenTrapHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (traps.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new SimpleAttack(null, MagicEffectType.BlackSpark, AnimatedTextColor.DarkRed, 30, 30) ) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );
                    } );
                }
            }

            return Promise.Completed;
        }
    }
}