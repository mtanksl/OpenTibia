using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Hole2Handler : CommandHandler<ItemMoveToTileCommand>
    {
        private HashSet<ushort> holes = new HashSet<ushort>() { 383, 469, 470, 482, 484, 485, 4835 };

        public override bool CanHandle(Context context, ItemMoveToTileCommand command)
        {
            if (command.ToTile.Ground != null && holes.Contains(command.ToTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemMoveToTileCommand command)
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

            context.AddCommand(new ItemMoveToTileCommand(command.Item, toTile) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}