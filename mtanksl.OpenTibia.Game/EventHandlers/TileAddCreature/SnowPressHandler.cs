﻿using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowPressHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private static Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
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

        private static Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>() 
        {
            { 6594, 670 },
            { 6595, 6580 },
            { 6596, 6581 },
            { 6597, 6582 },
            { 6598, 6583 },
            { 6599, 6584 },
            { 6600, 6585 },
            { 6601, 6586 },
            { 6602, 6587 },
            { 6603, 6588 },
            { 6604, 6589 },
            { 6605, 6590 },
            { 6606, 6591 },
            { 6607, 6592 },
            { 6608, 6593 }
        };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            if (e.ToTile.Ground != null && tiles.TryGetValue(e.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
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