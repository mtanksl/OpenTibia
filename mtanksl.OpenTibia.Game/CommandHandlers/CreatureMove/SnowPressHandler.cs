using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowPressHandler : CommandHandler<CreatureMoveCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 670, 6594 },
            { 6580, 6595 },
            { 6581, 6596 },
            { 6582, 6597 },
            { 6583, 6598 },
            { 6584, 6599 },
            { 6585, 6600 },
            { 6586, 6601 },
            { 6587, 6602 },
            { 6588, 6603 },
            { 6589, 6604 },
            { 6590, 6605 },
            { 6591, 6606 },
            { 6592, 6607 },
            { 6593, 6608 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(CreatureMoveCommand command, Server server)
        {
            if (command.ToTile.Ground != null && tiles.TryGetValue(command.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) && !command.Data.ContainsKey("SnowPressHandler") )
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
                command.Data.Add("SnowPressHandler", true);

                return context.TransformCommand(command);
            } ) );

            commands.Add(new ItemTransformCommand(command.ToTile.Ground, toOpenTibiaId, 1) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}