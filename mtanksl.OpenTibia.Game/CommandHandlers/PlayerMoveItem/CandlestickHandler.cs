using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CandlestickHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> candlestick = new HashSet<ushort>() { 2048 };

        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 2096, 2097 },

            { 6279, 6280 },

            { 5813, 5812 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (candlestick.Contains(command.Item.Metadata.OpenTibiaId) && command.ToContainer is Tile toTile && toTile.TopItem != null && transformations.TryGetValue(toTile.TopItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            context.AddCommand(new ItemDestroyCommand(command.Item) );

            context.AddCommand(new ItemReplaceCommand( ( (Tile)command.ToContainer).TopItem, toOpenTibiaId, 1) );

            OnComplete(context);
        }
    }
}