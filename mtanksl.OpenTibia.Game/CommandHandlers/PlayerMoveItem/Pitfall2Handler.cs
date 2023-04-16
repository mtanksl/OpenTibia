using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Pitfall2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private Dictionary<ushort, ushort> pitfalls = new Dictionary<ushort, ushort>()
        {
            {  293, 294 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            {  294, 293 }
        };

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
                            return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 0, command.Count, false) ).Then( () =>
                            {
                                return Context.AddCommand(new ItemTransformCommand(hole.Ground, toOpenTibiaId, 1) );

                            } ).Then( (item) =>
                            {
                                _ = Context.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[toOpenTibiaId], 1) );

                                return Promise.Completed;
                            } );
                        }
                    }
                }
            }

            return next();
        }
    }
}