using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TarHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> tars = new HashSet<ushort>() { 708, 709, 710, 711 };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && tars.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Puff) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemDecrementCommand(command.Item, command.Count) );
                } );
            }

            return next(context);
        }
    }
}