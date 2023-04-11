using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlatinumCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> platinumCoinsToCrystalCoin = new Dictionary<ushort, ushort>() 
        {
            { 2152, 2160 }
        };

        private Dictionary<ushort, ushort> platinumCoinToGoldCoins = new Dictionary<ushort, ushort>() 
        {
            { 2152, 2148 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (platinumCoinsToCrystalCoin.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 100)
            {
                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }
            else if (platinumCoinToGoldCoins.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 1)
            {
                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 100) );
            }

            return next();
        }
    }
}