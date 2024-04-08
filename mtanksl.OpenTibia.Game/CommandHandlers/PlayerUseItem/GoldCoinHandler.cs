using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GoldCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static Dictionary<ushort, ushort> goldCoinsToPlatinumCoin = new Dictionary<ushort, ushort>() 
        {
            { 2148, 2152 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (goldCoinsToPlatinumCoin.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 100)
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.BlueShimmer) ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                } );
            }

            return next();
        }
    }
}