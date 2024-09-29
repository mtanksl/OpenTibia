using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
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
            blueberryBushes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.blueberryBushes");
            decay = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.blueberryBushes");
            blueberry = Context.Server.Values.GetUInt16("values.items.blueberry");
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