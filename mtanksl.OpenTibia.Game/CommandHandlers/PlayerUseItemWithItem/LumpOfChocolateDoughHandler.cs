using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfChocolateDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> lumpOfChocolateDough = new HashSet<ushort>() { 8846 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort chocolateCake = 8847;

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (lumpOfChocolateDough.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
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

            commands.Add(new TileCreateItemCommand( (Tile)command.ToItem.Container, chocolateCake, 1) );
       
            return new SequenceCommand(commands.ToArray() );
        }
    }
}