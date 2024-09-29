using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Stairs2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly HashSet<ushort> stairs;

        public Stairs2Handler()
        {
            stairs = Context.Server.Values.GetUInt16HashSet("values.items.stairs");
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                Tile stair = tile;

                if (stair.TopItem != null && stairs.Contains(stair.TopItem.Metadata.OpenTibiaId) )
                {
                    Tile toTile;

                    if (stair.FloorChange == FloorChange.North)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(0, -1, -1) );
                    }
                    else if (stair.FloorChange == FloorChange.East)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(1, 0, -1) );
                    }
                    else if (stair.FloorChange == FloorChange.South)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(0, 1, -1) );
                    }
                    else if (stair.FloorChange == FloorChange.West)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(-1, 0, -1) );
                    }
                    else if (stair.FloorChange == FloorChange.NorthEast)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(1, -1, -1) );
                    }
                    else if (stair.FloorChange == FloorChange.NorthWest)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(-1, -1, -1) );
                    }
                    else if (stair.FloorChange == FloorChange.SouthWest)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(-1, 1, -1) );
                    }
                    else if (stair.FloorChange == FloorChange.SouthEast)
                    {
                        toTile = Context.Server.Map.GetTile(stair.Position.Offset(1, 1, -1) );
                    }
                    else
                    {
                        toTile = null;
                    }

                    if (toTile != null)
                    {
                        return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 255, command.Count, false) );
                    }
                }
            }

            return next();
        }
    }
}