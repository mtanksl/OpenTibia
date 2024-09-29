using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Hole2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly HashSet<ushort> holes;

        public Hole2Handler()
        {
            holes = Context.Server.Values.GetUInt16HashSet("values.items.holes");
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                Tile hole = tile;

                if (hole.Ground != null && holes.Contains(hole.Ground.Metadata.OpenTibiaId) )
                {
                    Tile down = Context.Server.Map.GetTile(hole.Position.Offset(0, 0, 1) );

                    if (down != null)
                    {
                        Tile toTile;

                        if (down.FloorChange == FloorChange.North)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(0, 1, 0) );
                        }
                        else if (down.FloorChange == FloorChange.East)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(-1, 0, 0) );
                        }
                        else if (down.FloorChange == FloorChange.South)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(0, -1, 0) );
                        }
                        else if (down.FloorChange == FloorChange.West)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(1, 0, 0) );
                        }
                        else if (down.FloorChange == FloorChange.NorthEast)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(-1, 1, 0) );
                        }
                        else if (down.FloorChange == FloorChange.NorthWest)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(1, 1, 0) );
                        }
                        else if (down.FloorChange == FloorChange.SouthWest)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(1, -1, 0) );
                        }
                        else if (down.FloorChange == FloorChange.SouthEast)
                        {
                            toTile = Context.Server.Map.GetTile(down.Position.Offset(-1, -1, 0) );
                        }
                        else
                        {
                            toTile = down;
                        }

                        if (toTile != null)
                        {
                            return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 255, command.Count, false) );
                        }
                    }
                }
            }

            return next();
        }
    }
}