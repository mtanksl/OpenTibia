using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileDepressHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 417, 416 },
            { 425, 426 },
            { 447, 446 },
            { 3217, 3216 }
        };

        public override void Handle(Context context, TileRemoveCreatureEventArgs e)
        {
            ushort toOpenTibiaId;
            
            Tile fromTile = e.Tile;

            if (fromTile.Ground != null && tiles.TryGetValue(fromTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                context.AddCommand(new ItemTransformCommand(fromTile.Ground, toOpenTibiaId, 1) );
            }
        }
    }
}