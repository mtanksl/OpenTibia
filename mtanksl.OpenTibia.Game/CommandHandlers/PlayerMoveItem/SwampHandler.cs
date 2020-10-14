using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SwampHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> swamps = new HashSet<ushort>() { 4691, 4692, 4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712 };

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && swamps.Contains(toTile.Ground.Metadata.OpenTibiaId) )
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

            commands.Add(new MagicEffectCommand( ( (Tile)command.ToContainer).Position, MagicEffectType.GreenRings) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}