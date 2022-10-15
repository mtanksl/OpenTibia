using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FireFieldHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> campfires = new HashSet<ushort>() { 1487, 1488, 1492, 1493 };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            Tile toTile = e.Tile;

            if (toTile.TopItem != null && campfires.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new CombatSelfAttackCommand(e.Creature, MagicEffectType.FirePlume, -20) );
            }
        }
    }
}