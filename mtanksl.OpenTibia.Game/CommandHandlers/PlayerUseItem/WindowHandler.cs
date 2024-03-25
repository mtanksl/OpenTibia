using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WindowHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            // Framework
            { 6438, 6436 },
            { 6436, 6438 },

            { 6439, 6437 },
            { 6437, 6439 },

            // Brick
            { 6442, 6440 },
            { 6440, 6442 },

            { 6443, 6441 },
            { 6441, 6443 },

            // Stone
            { 6446, 6444 },
            { 6444, 6446 },

            { 6447, 6445 },
            { 6445, 6447 },

            // Tree
            { 6452, 6450 },
            { 6450, 6452 },

            { 6453, 6451 },
            { 6451, 6453 },

            // Sandstone
            { 6456, 6454 },
            { 6454, 6456 },

            { 6457, 6455 },
            { 6455, 6457 },

            // Bamboo
            { 6460, 6458 },
            { 6458, 6460 },

            { 6461, 6459 },
            { 6459, 6461 },

            // Sandstone
            { 6464, 6462 },
            { 6462, 6464 },

            { 6465, 6463 },
            { 6463, 6465 },

            // Stone
            { 6468, 6466 },
            { 6466, 6468 },

            { 6469, 6467 },
            { 6467, 6469 },

            // Wooden
            { 6472, 6470 },
            { 6470, 6472 },

            { 6473, 6471 },
            { 6471, 6473 },

            // Fur
            { 6790, 6788 },
            { 6788, 6790 },

            { 6791, 6789 },
            { 6789, 6791 },

            // Nordic
            { 7027, 7025 },
            { 7025, 7027 },

            { 7028, 7026 },
            { 7026, 7028 },

            // Ice
            { 7031, 7029 },
            { 7029, 7031 },

            { 7032, 7030 },
            { 7030, 7032 },

            // Framework
            { 10264, 10266 },
            { 10266, 10264 },

            { 10265, 10267 },
            { 10267, 10265 },

            // Limestone
            { 10488, 10490 },
            { 10490, 10488 },
            { 10489, 10491 },
            { 10491, 10489 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.DoNotDisturb, 100, "Do Not Disturb") ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                } );
            }

            return next();
        }
    }
}