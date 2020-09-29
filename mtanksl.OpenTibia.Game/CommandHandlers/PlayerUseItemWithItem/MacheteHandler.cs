using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MacheteHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> machetes = new HashSet<ushort>() { 2420 };

        private Dictionary<ushort, ushort> jungleGrass = new Dictionary<ushort, ushort>()
        {
            { 2782, 2781 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if ( machetes.Contains(command.Item.Metadata.OpenTibiaId) && jungleGrass.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            return new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1);
        }
    }
}