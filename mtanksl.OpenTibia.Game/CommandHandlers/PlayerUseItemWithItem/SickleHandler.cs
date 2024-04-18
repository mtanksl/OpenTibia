using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SickleHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> sickles = new HashSet<ushort>() { 2405, 2418, 10513 };

        private static Dictionary<ushort, ushort> wheats = new Dictionary<ushort, ushort>()
        {
            { 5471, 5463 }
        };

        private static Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 5463, 5464 },
            { 5464, 5466 }
        };

        private static ushort bunchOfSugarCane = 5467;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (sickles.Contains(command.Item.Metadata.OpenTibiaId) && wheats.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.NaturalSweetener, 50, "Natural Sweetener") ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, bunchOfSugarCane, 1) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                } ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) ).Then( (item2) =>
                    {
                        _ = Context.AddCommand(new ItemDecayTransformCommand(item2, TimeSpan.FromSeconds(10), decay[item2.Metadata.OpenTibiaId], 1) );

                        return Promise.Completed;
                    } );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}