using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class Hole2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> holes = new HashSet<ushort>() { 383, 469, 470, 482, 484, 485, 4835 };

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && holes.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerMoveItemCommand command, Server server)
        {
            Tile toTile = (Tile)command.ToContainer;

            if (toTile.FloorChange == FloorChange.Down)
            {
                toTile = server.Map.GetTile(toTile.Position.Offset(0, 0, 1) );

                if (toTile.FloorChange == FloorChange.North)
                {
                    toTile = server.Map.GetTile(toTile.Position.Offset(0, 1, 0) );
                }
                else if (toTile.FloorChange == FloorChange.East)
                {
                    toTile = server.Map.GetTile(toTile.Position.Offset(-1, 0, 0) );
                }
                else if (toTile.FloorChange == FloorChange.South)
                {
                    toTile = server.Map.GetTile(toTile.Position.Offset(0, -1, 0) );
                }
                else if (toTile.FloorChange == FloorChange.West)
                {
                    toTile = server.Map.GetTile(toTile.Position.Offset(1, 0, 0) );
                }
            }

            return new CallbackCommand(context =>
            {
                return context.TransformCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 0, command.Count) );
            } );
        }
    }
}