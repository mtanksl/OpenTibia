using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LavaHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> lavas = new HashSet<ushort>() { 598, 599, 600, 601 };

        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && lavas.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            context.AddCommand(new ItemDecrementCountCommand( (StackableItem)command.Item, command.Count) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand( ( (Tile)command.ToContainer).Position, MagicEffectType.FirePlume) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}