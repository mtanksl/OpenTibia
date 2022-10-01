using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ShallowWaterHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625 };

        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            context.AddCommand(new ItemDecrementCountCommand(command.Item, command.Count) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand( ( (Tile)command.ToContainer).Position, MagicEffectType.BlueRings) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}