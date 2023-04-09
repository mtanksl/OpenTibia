using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemTransformHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            // Street lamp
            { 1479, 1480 },
            { 1480, 1479 },

            // Cockoo clock
            { 1873, 1874 },
            { 1874, 1873 },
            { 1875, 1876 },
            { 1876, 1875 },

            // Wall lamp
            { 2037, 2038 },
            { 2038, 2037 },
            { 2039, 2040 },
            { 2040, 2039 },

            // Wall lamp
            { 2068, 2069 },
            { 2069, 2068 },
            { 2066, 2067 },
            { 2067, 2066 },

            // Torch bearer
            { 2058, 2059 },
            { 2059, 2058 },
            { 2060, 2061 },
            { 2061, 2060 },

            // Lantern
            { 3743, 4404 },
            { 4404, 3743 },

            // Wall lamp
            { 3949, 3950 },
            { 3950, 3949 },
            { 3947, 3948 },
            { 3948, 3947 },

            // Torch bearer
            { 3943, 3944 },
            { 3944, 3943 },
            { 3945, 3946 },
            { 3946, 3945 },

            // Lever
            { 1945, 1946 },
            { 1946, 1945 },

            { 9825, 9826 },
            { 9826, 9825 },

            { 9827, 9828 },
            { 9828, 9827 },

            { 10029, 10030 },
            { 10030, 10029 },

            // Oven
            { 1786, 1787 },
            { 1787, 1786 },
            { 1788, 1789 },
            { 1789, 1788 },
            { 1790, 1791 },
            { 1791, 1790 },
            { 1792, 1793 },
            { 1793, 1792 },

            { 6356, 6357 },
            { 6357, 6356 },
            { 6358, 6359 },
            { 6359, 6358 },
            { 6360, 6361 },
            { 6361, 6360 },
            { 6362, 6363 },
            { 6363, 6362 },
            
            // Skull pillar
            { 7058, 7059 },
            { 7059, 7058 },

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

            // Magic lightwand
            { 2162, 2163 },
            { 2163, 2162 },

            // Trap
            { 2578, 2579 },
            { 2579, 2578 },

            //TODO: More items
        };

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next(context);
        }
    }
}