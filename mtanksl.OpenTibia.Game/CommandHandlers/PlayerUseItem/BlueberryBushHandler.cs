using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BlueberryBushHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> blueberryBushes;
        private readonly Dictionary<ushort, ushort> decay;
        private readonly ushort blueberry;

        public BlueberryBushHandler()
        {
            blueberryBushes = LuaScope.GetInt16Int16Dictionary(Context.Server.Values.GetValue("values.items.transformation.blueberryBushes") );
            decay = LuaScope.GetInt16Int16Dictionary(Context.Server.Values.GetValue("values.items.decay.blueberryBushes") );
            blueberry = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.blueberry") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (blueberryBushes.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.Bluebarian, 500, "Bluebarian") ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.Item.Parent, blueberry, 3) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            
                } ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}