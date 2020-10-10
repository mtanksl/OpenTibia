using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class HoleHandler : CommandHandler<CreatureMoveCommand>
    {
        private HashSet<ushort> holes = new HashSet<ushort>() { 383, 469, 470, 482, 484, 485, 4835 };

        public override bool CanHandle(CreatureMoveCommand command, Server server)
        {
            if ( command.ToTile.Ground != null && holes.Contains(command.ToTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(CreatureMoveCommand command, Server server)
        {
            Tile toTile = command.ToTile;

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
                return context.TransformCommand(new CreatureMoveCommand(command.Creature, toTile) );
            } );
        }
    }
}