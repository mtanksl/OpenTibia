using OpenTibia.Game.Events;
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

            if (e.Tile.Ground != null && tiles.TryGetValue(e.Tile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                context.AddCommand(new ItemTransformCommand(e.Tile.Ground, toOpenTibiaId, 1) );
            }
        }
    }
}