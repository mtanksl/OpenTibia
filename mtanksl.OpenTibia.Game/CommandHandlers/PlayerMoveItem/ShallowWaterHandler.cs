using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ShallowWaterHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625 };

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
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

            commands.Add(new MagicEffectCommand( ( (Tile)command.ToContainer).Position, MagicEffectType.BlueRings) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}