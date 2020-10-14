using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LavaHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> lavas = new HashSet<ushort>() { 598, 599, 600, 601 };

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && lavas.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerMoveItemCommand command, Server server)
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

            commands.Add(new MagicEffectCommand( ( (Tile)command.ToContainer).Position, MagicEffectType.FirePlume) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}