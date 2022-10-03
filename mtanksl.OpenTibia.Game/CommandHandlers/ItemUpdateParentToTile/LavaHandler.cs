using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LavaHandler : CommandHandler<ItemUpdateParentToTileCommand>
    {
        private HashSet<ushort> lavas = new HashSet<ushort>() { 598, 599, 600, 601 };

        public override bool CanHandle(Context context, ItemUpdateParentToTileCommand command)
        {
            if (command.ToTile.Ground != null && lavas.Contains(command.ToTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToTileCommand command)
        {
            context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand(command.ToTile.Position, MagicEffectType.FirePlume) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}