﻿using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileDepressHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        private static Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 417, 416 },
            { 425, 426 },
            { 447, 446 },
            { 3217, 3216 }
        };

        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            ushort toOpenTibiaId;
            
            if (e.FromTile.TopCreature == null && e.FromTile.Ground != null && tiles.TryGetValue(e.FromTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(e.FromTile.Ground, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}