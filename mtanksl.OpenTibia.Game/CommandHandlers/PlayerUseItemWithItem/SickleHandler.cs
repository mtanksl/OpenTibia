using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SickleHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> sickles;
        private readonly Dictionary<ushort, ushort> sugarCanes;
        private readonly Dictionary<ushort, ushort> decay;
        private readonly ushort bunchOfSugarCane;

        public SickleHandler()
        {
            sickles = Context.Server.Values.GetUInt16HashSet("values.items.sickles");
            sugarCanes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.sugarCanes");
            decay = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.sugarCanes");
            bunchOfSugarCane = Context.Server.Values.GetUInt16("values.items.bunchOfSugarCane");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (sickles.Contains(command.Item.Metadata.OpenTibiaId) && sugarCanes.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
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