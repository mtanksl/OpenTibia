using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfGarlicDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> lumpOfGarlicDoughs;
        private readonly HashSet<ushort> ovens;
        private readonly ushort garlicBread;
        private readonly HashSet<ushort> bakingTrays;
        private readonly ushort bakingTrayWithGarlicDough;

        public LumpOfGarlicDoughHandler()
        {
            lumpOfGarlicDoughs = Context.Server.Values.GetUInt16HashSet("values.items.lumpOfGarlicDoughs");
            ovens = Context.Server.Values.GetUInt16HashSet("values.items.ovens");
            garlicBread = Context.Server.Values.GetUInt16("values.items.garlicBread");
            bakingTrays = Context.Server.Values.GetUInt16HashSet("values.items.bakingTrays");
            bakingTrayWithGarlicDough = Context.Server.Values.GetUInt16("values.items.bakingTrayWithGarlicDough");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfGarlicDoughs.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, garlicBread, 1) );
                    } );
                }
                else if (bakingTrays.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.ToItem, bakingTrayWithGarlicDough, 1) );
                    } );
                }   
            }

            return next();
        }
    }
}