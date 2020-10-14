using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && command.ToContainer is Tile toTile)
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerMoveItemCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            commands.Add(new ItemDestroyCommand(command.Item) );

            commands.Add(new TileCreateItemCommand( (Tile)command.ToContainer, toOpenTibiaId, 1) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}