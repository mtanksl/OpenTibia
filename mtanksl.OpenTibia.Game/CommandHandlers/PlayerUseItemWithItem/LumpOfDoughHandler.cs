using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> lumpOfDoughs = new HashSet<ushort>() { 2693 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort bread = 2689;

        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfDoughs.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then(ctx =>
                {
                    return ctx.AddCommand(new TileIncrementOrCreateItemCommand( (Tile)command.ToItem.Parent, bread, 1) );
                } );
            }

            return next(context);
        }
    }
}