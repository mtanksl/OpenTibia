using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowPressHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> snowTiles;
        private readonly Dictionary<ushort, ushort> decay;

        public SnowPressHandler()
        {
            snowTiles = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.snowTiles");
            decay = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.snowTiles");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            if (e.ToTile.Ground != null && snowTiles.TryGetValue(e.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(e.ToTile.Ground, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return Promise.Completed;
        }
    }
}