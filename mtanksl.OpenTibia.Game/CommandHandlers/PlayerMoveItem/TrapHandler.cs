using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && command.ToContainer is Tile toTile)
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            context.AddCommand(new ItemDestroyCommand(command.Item) ).Then(ctx =>
            {
                return ctx.AddCommand(new ItemCreateCommand( (Tile)command.ToContainer, toOpenTibiaId, 1) );
            
            } ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}