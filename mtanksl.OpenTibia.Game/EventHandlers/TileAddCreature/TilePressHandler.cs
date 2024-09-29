using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TilePressHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> tilePress;

        public TilePressHandler()
        {
            tilePress = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.tilePress");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            if (e.ToTile.Ground != null && tilePress.TryGetValue(e.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(e.ToTile.Ground, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}