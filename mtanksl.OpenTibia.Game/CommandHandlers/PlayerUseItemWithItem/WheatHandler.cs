using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WheatHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> wheats = new HashSet<ushort>() { 2694 };

        private HashSet<ushort> millstones = new HashSet<ushort>() { 1381, 1382, 1383, 1384 };

        private ushort flour = 2692;

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (wheats.Contains(command.Item.Metadata.OpenTibiaId) && millstones.Contains(command.ToItem.Metadata.OpenTibiaId) )
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

            commands.Add(new TileCreateItemCommand(command.Player.Tile, flour, 1) );
     
            return new SequenceCommand(commands.ToArray() );
        }
    }
}