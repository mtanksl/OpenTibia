using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SnowHeapHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> snowHeaps = new HashSet<ushort>() { 486 };

        private static ushort snowBall = 2111;

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (snowHeaps.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerCreateItemCommand(command.Player, snowBall, 1) );
            }

            return next();
        }
    }
}