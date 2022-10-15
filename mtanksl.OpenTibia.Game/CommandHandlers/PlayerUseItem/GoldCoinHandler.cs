using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GoldCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> goldCoins = new Dictionary<ushort, ushort>() 
        {
            { 2148, 2152 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (goldCoins.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 100)
            {
                return context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next(context);
        }
    }
}