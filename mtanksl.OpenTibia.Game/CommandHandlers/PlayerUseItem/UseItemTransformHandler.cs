using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemTransformHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 6356, 6357 },
            { 6357, 6356 },
            { 6358, 6359 },
            { 6359, 6358 },
            { 6360, 6361 },
            { 6361, 6360 },
            { 6362, 6363 },
            { 6363, 6362 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(PlayerUseItemCommand command, Server server)
        {
            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Server server)
        {
            return new ItemTransformCommand(command.Item, toOpenTibiaId);
        }
    }
}