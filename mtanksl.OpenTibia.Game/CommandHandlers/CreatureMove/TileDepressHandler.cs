using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileDepressHandler : CommandHandler<CreatureMoveCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 417, 416 },
            { 425, 426 },
            { 447, 446 },
            { 3217, 3216 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(CreatureMoveCommand command, Server server)
        {
            if ( !command.Data.ContainsKey("TileDepressHandler") && command.Creature.Tile.Ground != null && tiles.TryGetValue(command.Creature.Tile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(CreatureMoveCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            commands.Add(new ItemTransformCommand(command.Creature.Tile.Ground, toOpenTibiaId, 1) );

            commands.Add(new CallbackCommand(context =>
            {
                command.Data.Add("TileDepressHandler", true);

                return context.TransformCommand(command);
            } ) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}