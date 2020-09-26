using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemTransformHandler : CommandHandler<PlayerRotateItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 6356, 6358 },
            { 6358, 6360 },
            { 6360, 6362 },
            { 6362, 6356 },

            { 6357, 6359 },
            { 6359, 6361 },
            { 6361, 6363 },
            { 6363, 6357 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(PlayerRotateItemCommand command, Context context)
        {
            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerRotateItemCommand command, Context context)
        {
            return new ItemTransformCommand(command.Item, toOpenTibiaId);
        }
    }
}