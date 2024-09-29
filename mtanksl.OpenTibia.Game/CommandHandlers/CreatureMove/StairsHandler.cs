using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StairsHandler : CommandHandler<CreatureMoveCommand>
    {
        private readonly HashSet<ushort> stairs;

        public StairsHandler()
        {
            stairs = Context.Server.Values.GetUInt16HashSet("values.items.stairs");
        }

        public override Promise Handle(Func<Promise> next, CreatureMoveCommand command)
        {
            Tile stair = command.ToTile;

            if (stair.TopItem != null && stairs.Contains(stair.TopItem.Metadata.OpenTibiaId) )
            {
                Tile toTile;

                Direction direction;

                if (stair.FloorChange == FloorChange.North)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(0, -1, -1) );

                    direction = Direction.North;
                }
                else if (stair.FloorChange == FloorChange.East)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(1, 0, -1) );

                    direction = Direction.East;
                }
                else if (stair.FloorChange == FloorChange.South)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(0, 1, -1) );

                    direction = Direction.South;
                }
                else if (stair.FloorChange == FloorChange.West)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(-1, 0, -1) );

                    direction = Direction.West;
                }
                else if (stair.FloorChange == FloorChange.NorthEast)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(1, -1, -1) );

                    direction = Direction.East;
                }
                else if (stair.FloorChange == FloorChange.NorthWest)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(-1, -1, -1) );

                    direction = Direction.West;
                }
                else if (stair.FloorChange == FloorChange.SouthWest)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(-1, 1, -1) );

                    direction = Direction.West;
                }
                else if (stair.FloorChange == FloorChange.SouthEast)
                {
                    toTile = Context.Server.Map.GetTile(stair.Position.Offset(1, 1, -1) );

                    direction = Direction.East;
                }
                else
                {
                    toTile = null;

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

            return next();
        }
    }
}