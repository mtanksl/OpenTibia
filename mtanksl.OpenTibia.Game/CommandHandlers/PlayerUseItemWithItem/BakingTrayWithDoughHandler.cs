using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BakingTrayWithDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> bakingTrayWithDoughs;
        private readonly HashSet<ushort> ovens;
        private readonly ushort bakingTray;
        private readonly ushort cookie;

        public BakingTrayWithDoughHandler()
        {
            bakingTrayWithDoughs = Context.Server.Values.GetUInt16HashSet("values.items.bakingTrayWithDoughs");
            ovens = Context.Server.Values.GetUInt16HashSet("values.items.ovens");
            bakingTray = Context.Server.Values.GetUInt16("values.items.bakingTray");
            cookie = Context.Server.Values.GetUInt16("values.items.cookie");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (bakingTrayWithDoughs.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.CookieMonster, 20, "Cookie Monster") ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, bakingTray, 1) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, cookie, 12) );
                } );
            }

            return next();
        }
    }
}