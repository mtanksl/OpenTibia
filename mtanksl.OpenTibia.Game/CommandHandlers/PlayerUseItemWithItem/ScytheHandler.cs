using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ScytheHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> scythes = new HashSet<ushort>() { 2550, 10513 };

        private Dictionary<ushort, ushort> wheats = new Dictionary<ushort, ushort>()
        {
            { 2739, 2737 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 2737, 2738 },
            { 2738, 2739 }
        };

        private ushort wheat = 2694;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (scythes.Contains(command.Item.Metadata.OpenTibiaId) && wheats.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.HappyFarmer, 200, "Happy Farmer") ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, wheat, 1) );
                
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