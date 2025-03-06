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
        private readonly ushort bunchOfSugarCane;

        public SickleHandler()
        {
            sickles = Context.Server.Values.GetUInt16HashSet("values.items.sickles");
            sugarCanes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.sugarCanes");
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
                } );
            }

            return next();
        }
    }
}