using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CrystalCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> crystalCoinToPlatinumCoins = new Dictionary<ushort, ushort>() 
        {
            { 2160, 2152 }
        };

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (crystalCoinToPlatinumCoins.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 1)
            {
                return context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 100) );
            }

            return next(context);
        }
    }
}