using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> lumpOfDoughs = new HashSet<ushort>() { 2693 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort bread = 2689;

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (lumpOfDoughs.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId))
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            if (command.Item is StackableItem stackableItem && stackableItem.Count > 1)
            {
                commands.Add(new ItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count - 1) ) );
            }
            else
            {
                commands.Add(new ItemDestroyCommand(command.Item) );
            }

            commands.Add(new TileCreateItemCommand( (Tile)command.ToItem.Container, bread, 1) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}