using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileDepressHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> tileDepress;

        public TileDepressHandler()
        {
            tileDepress = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.tileDepress");
        }

        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            ushort toOpenTibiaId;
            
            if (e.FromTile.TopCreature == null && e.FromTile.Ground != null && tileDepress.TryGetValue(e.FromTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(e.FromTile.Ground, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}