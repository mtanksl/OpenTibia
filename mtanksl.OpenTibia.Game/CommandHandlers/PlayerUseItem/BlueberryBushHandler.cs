using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BlueberryBushHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> blueberryBushs = new Dictionary<ushort, ushort>() 
        {
            { 2785, 2786 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>() 
        {
            { 2786, 2785 }
        };

        private ushort blueberry = 2677;

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (blueberryBushs.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new TileCreateItemCommand( (Tile)command.Item.Parent, blueberry, 3) ).Then( (ctx, item) =>
                {
                    return ctx.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            
                } ).Then( (ctx, item) =>
                {
                    return ctx.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[item.Metadata.OpenTibiaId], 1) );                      
                } );
            }

            return next(context);
        }
    }
}