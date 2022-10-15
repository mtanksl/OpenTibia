using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlatinumCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> platinumCoins = new Dictionary<ushort, ushort>() 
        {
            { 2152, 2160 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (platinumCoins.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 100)
            {
                return context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next(context);
        }
    }
}