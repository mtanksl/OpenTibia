using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Pitfall2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly Dictionary<ushort, ushort> pitfalls;

        public Pitfall2Handler()
        {
            pitfalls = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.pitfalls");
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                ushort toOpenTibiaId;

                Tile hole = tile;

                if (hole.Ground != null && pitfalls.TryGetValue(hole.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
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
                            return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 255, command.Count, false) ).Then( () =>
                            {
                                return Context.AddCommand(new ItemTransformCommand(hole.Ground, toOpenTibiaId, 1) );
                            } );
                        }
                    }
                }
            }

            return next();
        }
    }
}