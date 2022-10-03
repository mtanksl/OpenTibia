using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapHandler : CommandHandler<ItemMoveToTileCommand>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, ItemMoveToTileCommand command)
        {
            if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemMoveToTileCommand command)
        {
            context.AddCommand(new ItemDestroyCommand(command.Item) ).Then(ctx =>
            {
                return ctx.AddCommand(new TileCreateItemCommand(command.ToTile, toOpenTibiaId, 1) );
            
            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}