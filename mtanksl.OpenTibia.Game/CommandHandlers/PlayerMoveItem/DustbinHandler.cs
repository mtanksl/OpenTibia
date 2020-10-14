using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DustbinHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> dustbins = new HashSet<ushort>() { 1777 };

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (command.ToContainer is Tile toTile && toTile.TopItem != null && dustbins.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerMoveItemCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            if (command.Item is StackableItem stackableItem && stackableItem.Count > command.Count)
            {
                commands.Add(new ItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count - command.Count) ) );
            }
            else
            {
                commands.Add(new ItemDestroyCommand(command.Item) );
            }

            return new SequenceCommand(commands.ToArray() );
        }
    }
}