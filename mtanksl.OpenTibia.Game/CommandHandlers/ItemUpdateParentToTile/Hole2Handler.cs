using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Hole2Handler : CommandHandler<ItemUpdateParentToTileCommand>
    {
        private HashSet<ushort> holes = new HashSet<ushort>() 
        { 
            383, 469, 470, 482, 484, 485, 369, 409, 410, 411, 

            459, 

            369, 409, 410, 411,

            423, 427, 428, 429,

            432, 433,

            3135, 3136, 3137, 3138,

            3219, 3220,

            4834, 4835, 

            4836, 4837,

            475, 476, 479, 480, 6127, 6128, 6129, 6130,

            6917, 6918, 6919, 6920, 6921, 6922, 6923, 6924,

            8559, 8560, 8561, 8562, 8563, 8564, 8565, 8566
        };

        public override bool CanHandle(Context context, ItemUpdateParentToTileCommand command)
        {
            if (command.ToTile.Ground != null && holes.Contains(command.ToTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToTileCommand command)
        {
            Tile toTile = command.ToTile;

            if (toTile.FloorChange == FloorChange.Down)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(0, 0, 1) );

                if (toTile.FloorChange == FloorChange.North)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(0, 1, 0) );
                }
                else if (toTile.FloorChange == FloorChange.East)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(-1, 0, 0) );
                }
                else if (toTile.FloorChange == FloorChange.South)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(0, -1, 0) );
                }
                else if (toTile.FloorChange == FloorChange.West)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(1, 0, 0) );
                }
                else if (toTile.FloorChange == FloorChange.NorthEast)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(-1, 1, 0) );
                }
                else if (toTile.FloorChange == FloorChange.NorthWest)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(1, 1, 0) );
                }
                else if (toTile.FloorChange == FloorChange.SouthWest)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(1, -1, 0) );
                }
                else if (toTile.FloorChange == FloorChange.SouthEast)
                {
                    toTile = context.Server.Map.GetTile(toTile.Position.Offset(-1, -1, 0) );
                }
            }

            context.AddCommand(new ItemUpdateParentToTileCommand(command.Item, toTile) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}