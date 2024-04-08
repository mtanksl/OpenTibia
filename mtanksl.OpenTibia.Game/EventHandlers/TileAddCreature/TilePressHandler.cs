using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TilePressHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private static Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 416, 417 },
            { 426, 425 },
            { 446, 447 },
            { 3216, 3217 }
        };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            if (e.ToTile.Ground != null && tiles.TryGetValue(e.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(e.ToTile.Ground, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}