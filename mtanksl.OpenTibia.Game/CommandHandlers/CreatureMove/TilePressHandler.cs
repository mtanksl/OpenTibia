using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TilePressHandler : CommandHandler<CreatureMoveCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 416, 417 },
            { 426, 425 },
            { 446, 447 },
            { 3216, 3217 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(CreatureMoveCommand command, Server server)
        {
            if ( !command.Data.ContainsKey("TilePressHandler") && command.ToTile.Ground != null && tiles.TryGetValue(command.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(CreatureMoveCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            commands.Add(new CallbackCommand(context =>
            {
                command.Data.Add("TilePressHandler", true);

                return context.TransformCommand(command);
            } ) );

            commands.Add(new ItemTransformCommand(command.ToTile.Ground, toOpenTibiaId, 1) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}