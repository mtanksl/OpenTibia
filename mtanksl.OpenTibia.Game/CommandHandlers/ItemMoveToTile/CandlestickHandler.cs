using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CandlestickHandler : CommandHandler<ItemMoveToTileCommand>
    {
        private HashSet<ushort> candlestick = new HashSet<ushort>() { 2048 };

        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 2096, 2097 },

            { 6279, 6280 },

            { 5813, 5812 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, ItemMoveToTileCommand command)
        {
            if (candlestick.Contains(command.Item.Metadata.OpenTibiaId) && command.ToTile.TopItem != null && transformations.TryGetValue(command.ToTile.TopItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemMoveToTileCommand command)
        {
            context.AddCommand(new ItemDestroyCommand(command.Item) ).Then(ctx =>
            {
                return ctx.AddCommand(new ItemTransformCommand(command.ToTile.TopItem, toOpenTibiaId, 1) );
          
            } ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );           
        }
    }
}