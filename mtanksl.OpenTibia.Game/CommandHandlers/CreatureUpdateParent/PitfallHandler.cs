using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PitfallHandler : CommandHandler<CreatureUpdateParentCommand>
    {
        private Dictionary<ushort, ushort> pitfalls = new Dictionary<ushort, ushort>() 
        {
            {  293, 294 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            {  294, 293 }
        };

        public override Promise Handle(Func<Promise> next, CreatureUpdateParentCommand command)
        {
            ushort toOpenTibiaId;

            Tile hole = command.ToTile;

            if (hole.Ground != null && pitfalls.TryGetValue(hole.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
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
                        return Context.AddCommand(new CreatureUpdateParentCommand(command.Creature, toTile, direction) ).Then( () =>
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

            return next();
        }
    }
}