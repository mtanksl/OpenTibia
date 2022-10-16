using OpenTibia.Common.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CampfireHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> campfires = new HashSet<ushort>() { 1423, 1424, 1425 };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            if (e.Tile.TopItem != null && campfires.Contains(e.Tile.TopItem.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new CombatDirectAttackCommand(e.Creature, MagicEffectType.FirePlume, -20) );
            }
        }
    }
}