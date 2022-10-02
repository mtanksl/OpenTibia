using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ScytheHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> scythes = new HashSet<ushort>() { 2550 };

        private Dictionary<ushort, ushort> wheats = new Dictionary<ushort, ushort>()
        {
            { 2739, 2737 }
        };

        private ushort wheat = 2694;

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (scythes.Contains(command.Item.Metadata.OpenTibiaId) && wheats.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            context.AddCommand(new TileCreateItemCommand( (Tile)command.ToItem.Parent, wheat, 1) ).Then(ctx =>
            {
                return ctx.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

            } ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}