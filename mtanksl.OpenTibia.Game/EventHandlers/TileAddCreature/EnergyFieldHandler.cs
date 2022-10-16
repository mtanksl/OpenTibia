using OpenTibia.Common.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class EnergyFieldHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> campfires = new HashSet<ushort>() { 1491, 1495 };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            if (e.Tile.TopItem != null && campfires.Contains(e.Tile.TopItem.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new CombatDirectAttackCommand(e.Creature, MagicEffectType.EnergyDamage, -30) );
            }
        }
    }
}