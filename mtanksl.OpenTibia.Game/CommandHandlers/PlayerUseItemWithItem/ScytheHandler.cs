using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ScytheHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> scythes = new HashSet<ushort>() { 2550 };

        private Dictionary<ushort, ushort> wheats = new Dictionary<ushort, ushort>()
        {
            { 2739, 2737 }
        };

        private ushort wheat = 2694;

        private ushort toOpenTibiaId;

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (scythes.Contains(command.Item.Metadata.OpenTibiaId) && wheats.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            commands.Add(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

            commands.Add(new TileCreateItemCommand( (Tile)command.ToItem.Container, wheat, 1) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}