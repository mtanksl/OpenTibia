using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StairsHandler : CommandHandler<CreatureMoveCommand>
    {
        private HashSet<ushort> stairs = new HashSet<ushort>() { 1385, 5258, 1396, 8709, 3687, 3688, 5259, 5260 };

        public override bool CanHandle(CreatureMoveCommand command, Server server)
        {
            if (command.ToTile.TopItem != null && stairs.Contains(command.ToTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(CreatureMoveCommand command, Server server)
        {
            Tile toTile = command.ToTile;

            if (toTile.FloorChange == FloorChange.North)
            {
                toTile = server.Map.GetTile(toTile.Position.Offset(0, -1, -1) );
            }
            else if (toTile.FloorChange == FloorChange.East)
            {
                toTile = server.Map.GetTile(toTile.Position.Offset(1, 0, -1) );
            }
            else if (toTile.FloorChange == FloorChange.South)
            {
                toTile = server.Map.GetTile(toTile.Position.Offset(0, 1, -1) );
            }
            else if (toTile.FloorChange == FloorChange.West)
            {
                toTile = server.Map.GetTile(toTile.Position.Offset(-1, 0, -1) );
            }

            return new CallbackCommand(context =>
            {
                return context.TransformCommand(new CreatureMoveCommand(command.Creature, toTile) );
            } );
        }
    }
}