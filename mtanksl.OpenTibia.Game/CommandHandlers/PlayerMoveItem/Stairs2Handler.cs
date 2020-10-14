using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Stairs2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> stairs = new HashSet<ushort>() { 1385, 5258, 1396, 8709, 3687, 3688, 5259, 5260 };

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (command.ToContainer is Tile toTile && toTile.TopItem != null && stairs.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerMoveItemCommand command, Server server)
        {
            Tile toTile = (Tile)command.ToContainer;

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
                return context.TransformCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 0, command.Count) );
            } );
        }
    }
}