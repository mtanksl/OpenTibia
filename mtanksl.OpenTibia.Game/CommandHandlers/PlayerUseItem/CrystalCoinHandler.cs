using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CrystalCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static Dictionary<ushort, ushort> crystalCoinToPlatinumCoins = new Dictionary<ushort, ushort>() 
        {
            { 2160, 2152 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (crystalCoinToPlatinumCoins.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 1)
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.BlueShimmer) ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 100) );
                } );
            }

            return next();
        }
    }
}