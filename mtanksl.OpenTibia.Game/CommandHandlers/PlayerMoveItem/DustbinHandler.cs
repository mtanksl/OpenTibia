using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DustbinHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> dustbins = new HashSet<ushort>() { 1777 };

        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.TopItem != null && dustbins.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            context.AddCommand(new ItemDecrementCommand(command.Item, command.Count) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}