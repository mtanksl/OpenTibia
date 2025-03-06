using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowPressHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> snowTiles;

        public SnowPressHandler()
        {
            snowTiles = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.snowTiles");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            if (e.ToTile.Ground != null && snowTiles.TryGetValue(e.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(e.ToTile.Ground, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}