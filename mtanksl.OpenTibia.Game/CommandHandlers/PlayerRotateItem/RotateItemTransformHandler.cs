using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemTransformHandler : CommandHandler<PlayerRotateItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
             // Oven
            { 6356, 6358 },
            { 6358, 6360 },
            { 6360, 6362 },
            { 6362, 6356 },

            { 6357, 6359 },
            { 6359, 6361 },
            { 6361, 6363 },
            { 6363, 6357 },

            // Wooden chair
            { 1650, 1651 },
            { 1651, 1652 },
            { 1652, 1653 },
            { 1653, 1650 },

            // Sofa chair
            { 1658, 1659 },
            { 1659, 1660 },
            { 1660, 1661 },
            { 1661, 1658 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerRotateItemCommand command)
        {
            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerRotateItemCommand command)
        {
            context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}