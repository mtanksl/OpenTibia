using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ShallowWaterHandler : CommandHandler<ItemUpdateParentToTileCommand>
    {
        private HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625 };

        public override bool CanHandle(Context context, ItemUpdateParentToTileCommand command)
        {
            if (command.ToTile.Ground != null && shallowWaters.Contains(command.ToTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToTileCommand command)
        {
            context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand(command.ToTile.Position, MagicEffectType.BlueRings) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}