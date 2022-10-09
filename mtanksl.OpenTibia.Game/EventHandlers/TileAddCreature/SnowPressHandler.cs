using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowPressHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 670, 6594 },
            { 6580, 6595 },
            { 6581, 6596 },
            { 6582, 6597 },
            { 6583, 6598 },
            { 6584, 6599 },
            { 6585, 6600 },
            { 6586, 6601 },
            { 6587, 6602 },
            { 6588, 6603 },
            { 6589, 6604 },
            { 6590, 6605 },
            { 6591, 6606 },
            { 6592, 6607 },
            { 6593, 6608 }
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