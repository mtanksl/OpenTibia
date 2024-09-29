using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BakingTrayWithGarlicDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> bakingTrayWithGarlicDoughs;
        private readonly HashSet<ushort> ovens;
        private readonly ushort bakingTray;
        private readonly ushort garlicCookie;

        public BakingTrayWithGarlicDoughHandler()
        {
            bakingTrayWithGarlicDoughs = Context.Server.Values.GetUInt16HashSet("values.items.bakingTrayWithGarlicDoughs");
            ovens = Context.Server.Values.GetUInt16HashSet("values.items.ovens");
            bakingTray = Context.Server.Values.GetUInt16("values.items.bakingTray");
            garlicCookie = Context.Server.Values.GetUInt16("values.items.garlicCookie");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (bakingTrayWithGarlicDoughs.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ItemDestroyCommand(command.Item) ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, bakingTray, 1) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, garlicCookie, 12) );
                } );
            }

            return next();
        }
    }
}