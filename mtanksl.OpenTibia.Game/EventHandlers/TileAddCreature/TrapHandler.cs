using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        public override void Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (traps.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    context.AddCommand(CombatCommand.TargetAttack(null, e.Creature, null, MagicEffectType.BlackSpark, (attacker, target) => -30) );

                    context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );

                    break;
                }
            }
        }
    }
}