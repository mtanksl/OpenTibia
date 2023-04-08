using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowHeapHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> snowHeaps = new HashSet<ushort>() { 486 };

        private ushort snowBall = 2111;

        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            if (snowHeaps.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new TileIncrementOrCreateItemCommand(command.Player.Tile, snowBall, 1) );
            }

            return next(context);
        }
    }
}