using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemTransformHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            // Oven
            { 1786, 1787 },
            { 1787, 1786 },
            { 1788, 1789 },
            { 1789, 1788 },
            { 1790, 1791 },
            { 1791, 1790 },
            { 1792, 1793 },
            { 1793, 1792 },

            // Oven
            { 6356, 6357 },
            { 6357, 6356 },
            { 6358, 6359 },
            { 6359, 6358 },
            { 6360, 6361 },
            { 6361, 6360 },
            { 6362, 6363 },
            { 6363, 6362 },

            // Lever
            { 1945, 1946 },
            { 1946, 1945 },

            { 9825, 9826 },
            { 9826, 9825 },

            { 9827, 9828 },
            { 9828, 9827 },

            { 10029, 10030 },
            { 10030, 10029 },

            // Torch
            { 2050, 2051 },
            { 2051, 2050 },

            { 2052, 2053 },
            { 2053, 2052 },

            { 2054, 2055 },
            { 2055, 2054 },

            // Candelabrum
            { 2041, 2042 },
            { 2042, 2041 },

            // Lamp
            { 2044, 2045 },
            { 2045, 2044 },

            // Candlestick
            { 2047, 2048 },
            { 2048, 2047 },

            // Trap
            { 2578, 2579 },
            { 2579, 2578 },
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerUseItemCommand command)
        {
            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemCommand command)
        {
            context.AddCommand(new ItemReplaceCommand(command.Item, toOpenTibiaId, 1) );

            OnComplete(context);
        }
    }
}