using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class HoleHandler : CommandHandler<CreatureMoveCommand>
    {
        private readonly HashSet<ushort> holes;

        public HoleHandler()
        {
            holes = Context.Server.Values.GetUInt16HashSet("values.items.holes");
        }

        public override Promise Handle(Func<Promise> next, CreatureMoveCommand command)
        {
            Tile hole = command.ToTile;

            if (hole.Ground != null && holes.Contains(hole.Ground.Metadata.OpenTibiaId) )
            {
                Tile down = Context.Server.Map.GetTile(hole.Position.Offset(0, 0, 1) );

                if (down != null)
                {
                    Tile toTile;

                    Direction direction;

                    if (down.FloorChange == FloorChange.North)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(0, 1, 0) );

                        direction = Direction.South;
                    }
                    else if (down.FloorChange == FloorChange.East)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(-1, 0, 0) );

                        direction = Direction.West;
                    }
                    else if (down.FloorChange == FloorChange.South)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(0, -1, 0) );

                        direction = Direction.North;
                    }
                    else if (down.FloorChange == FloorChange.West)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(1, 0, 0) );

                        direction = Direction.East;
                    }
                    else if (down.FloorChange == FloorChange.NorthEast)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(-1, 1, 0) );

                        direction = Direction.West;
                    }
                    else if (down.FloorChange == FloorChange.NorthWest)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(1, 1, 0) );

                        direction = Direction.East;
                    }
                    else if (down.FloorChange == FloorChange.SouthWest)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(1, -1, 0) );

                        direction = Direction.East;
                    }
                    else if (down.FloorChange == FloorChange.SouthEast)
                    {
                        toTile = Context.Server.Map.GetTile(down.Position.Offset(-1, -1, 0) );

                        direction = Direction.West;
                    }
                    else
                    {
                        toTile = down;

                        direction = Direction.South;
                    }

                    if (toTile != null)
                    {
                        return Context.AddCommand(new CreatureMoveCommand(command.Creature, toTile) ).Then( () =>
                        {
                            return Context.AddCommand(new CreatureUpdateDirectionCommand(command.Creature, direction) );
                        } );
                    }
                }
            }

            return next();
        }
    }
}