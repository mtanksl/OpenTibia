using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Hole2Handler : CommandHandler<TileAddItemCommand>
    {
        private HashSet<ushort> holes = new HashSet<ushort>() 
        { 
            // Pitfall
            294,

            // Hole
            383, 469, 470, 482, 484, 485, 369, 409, 410, 411, 

            // Invisible
            459, 

            // Wooden
            369, 409, 410, 411,

            // Wooden
            423, 427, 428, 429,

            // White marble
            432, 433,

            // Dark wooden
            3135, 3136, 3137, 3138,

            // Stone
            3219, 3220,

            // Black marble
            4834, 4835, 

            // Gray marble
            4836, 4837,

            // Stone
            475, 476, 479, 480, 6127, 6128, 6129, 6130,

            // Snow
            6917, 6918, 6919, 6920, 6921, 6922, 6923, 6924,

            // Earth
            8559, 8560, 8561, 8562, 8563, 8564, 8565, 8566
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, TileAddItemCommand command)
        {
            Tile hole = command.Tile;

            if (hole.Ground != null && holes.Contains(hole.Ground.Metadata.OpenTibiaId) )
            {
                Tile down = context.Server.Map.GetTile(hole.Position.Offset(0, 0, 1) );

                if (down != null)
                {
                    Tile toTile;

                    if (down.FloorChange == FloorChange.North)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(0, 1, 0) );
                    }
                    else if (down.FloorChange == FloorChange.East)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(-1, 0, 0) );
                    }
                    else if (down.FloorChange == FloorChange.South)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(0, -1, 0) );
                    }
                    else if (down.FloorChange == FloorChange.West)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(1, 0, 0) );
                    }
                    else if (down.FloorChange == FloorChange.NorthEast)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(-1, 1, 0) );
                    }
                    else if (down.FloorChange == FloorChange.NorthWest)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(1, 1, 0) );
                    }
                    else if (down.FloorChange == FloorChange.SouthWest)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(1, -1, 0) );
                    }
                    else if (down.FloorChange == FloorChange.SouthEast)
                    {
                        toTile = context.Server.Map.GetTile(down.Position.Offset(-1, -1, 0) );
                    }
                    else
                    {
                        toTile = down;
                    }

                    if (toTile != null)
                    {
                        return context.AddCommand(new TileAddItemCommand(toTile, command.Item) );
                    }
                }
            }

            return next(context);
        }
    }
}