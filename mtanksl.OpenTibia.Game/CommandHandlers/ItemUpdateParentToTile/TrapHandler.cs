using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapHandler : CommandHandler<ItemUpdateParentToTileCommand>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, ItemUpdateParentToTileCommand command)
        {
            if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToTileCommand command)
        {
            context.AddCommand(new ItemDestroyCommand(command.Item) ).Then(ctx =>
            {
                return ctx.AddCommand(new TileCreateItemCommand(command.ToTile, toOpenTibiaId, 1) );
            
            } ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}