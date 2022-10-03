using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SwampHandler : CommandHandler<ItemUpdateParentToTileCommand>
    {
        private HashSet<ushort> swamps = new HashSet<ushort>() { 4691, 4692, 4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712 };

        public override bool CanHandle(Context context, ItemUpdateParentToTileCommand command)
        {
            if (command.ToTile.Ground != null && swamps.Contains(command.ToTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToTileCommand command)
        {
            context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand(command.ToTile.Position, MagicEffectType.GreenRings) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}