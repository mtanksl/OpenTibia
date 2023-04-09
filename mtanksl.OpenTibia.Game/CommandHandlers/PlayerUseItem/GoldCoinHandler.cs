using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GoldCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> goldCoinsToPlatinumCoin = new Dictionary<ushort, ushort>() 
        {
            { 2148, 2152 }
        };

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (goldCoinsToPlatinumCoin.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 100)
            {
                return context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next(context);
        }
    }
}