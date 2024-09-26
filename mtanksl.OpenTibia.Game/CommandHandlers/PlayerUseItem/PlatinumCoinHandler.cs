using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlatinumCoinHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> platinumCoinToGoldCoin;
        private readonly Dictionary<ushort, ushort> platinumCoinToCrystalCoin;

        public PlatinumCoinHandler()
        {
            platinumCoinToGoldCoin = LuaScope.GetInt16Int16Dictionary(Context.Server.Values.GetValue("values.items.transformation.platinumCoinToGoldCoin") );
            platinumCoinToCrystalCoin = LuaScope.GetInt16Int16Dictionary(Context.Server.Values.GetValue("values.items.transformation.platinumCoinToCrystalCoin") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (platinumCoinToCrystalCoin.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 100)
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.BlueShimmer) ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                } );                
            }
            else if (platinumCoinToGoldCoin.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && ( (StackableItem)command.Item).Count == 1)
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