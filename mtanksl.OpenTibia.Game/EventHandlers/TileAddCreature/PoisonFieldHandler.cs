using OpenTibia.Common.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PoisonFieldHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> campfires = new HashSet<ushort>() { 1490, 1496 };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            if (e.Tile.TopItem != null && campfires.Contains(e.Tile.TopItem.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new CombatDirectAttackCommand(e.Creature, MagicEffectType.GreenRings, -5) );
            }
        }
    }
}