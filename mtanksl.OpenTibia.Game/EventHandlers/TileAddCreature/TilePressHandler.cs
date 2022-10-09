using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TilePressHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 416, 417 },
            { 426, 425 },
            { 446, 447 },
            { 3216, 3217 }
        };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            Tile toTile = e.Tile;

            if (toTile.Ground != null && tiles.TryGetValue(toTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                context.AddCommand(new ItemTransformCommand(toTile.Ground, toOpenTibiaId, 1) );
            }
        }
    }
}