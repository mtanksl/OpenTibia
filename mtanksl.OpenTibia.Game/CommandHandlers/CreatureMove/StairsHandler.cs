using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StairsHandler : CommandHandler<CreatureMoveCommand>
    {
        private HashSet<ushort> stairs = new HashSet<ushort>() { 1385, 5258, 1396, 8709, 3687, 3688, 5259, 5260 };

        public override bool CanHandle(Context context, CreatureMoveCommand command)
        {
            if (command.ToTile.TopItem != null && stairs.Contains(command.ToTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureMoveCommand command)
        {
            Tile toTile = command.ToTile;

            if (toTile.FloorChange == FloorChange.North)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(0, -1, -1) );
            }
            else if (toTile.FloorChange == FloorChange.East)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(1, 0, -1) );
            }
            else if (toTile.FloorChange == FloorChange.South)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(0, 1, -1) );
            }
            else if (toTile.FloorChange == FloorChange.West)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(-1, 0, -1) );
            }
            else if (toTile.FloorChange == FloorChange.NorthEast)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(1, -1, -1) );
            }
            else if (toTile.FloorChange == FloorChange.NorthWest)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(-1, -1, -1) );
            }
            else if (toTile.FloorChange == FloorChange.SouthWest)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(-1, 1, -1) );
            }
            else if (toTile.FloorChange == FloorChange.SouthEast)
            {
                toTile = context.Server.Map.GetTile(toTile.Position.Offset(1, 1, -1) );
            }

            context.AddCommand(new CreatureMoveCommand(command.Creature, toTile), ctx =>
            {
                base.Handle(ctx, command);
            } );
        }
    }
}