using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TilePressHandler : CommandHandler<CreatureUpdateParentCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 416, 417 },
            { 426, 425 },
            { 446, 447 },
            { 3216, 3217 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, CreatureUpdateParentCommand command)
        {
            ushort toOpenTibiaId;

            Tile toTile = command.ToTile;

            if (toTile.Ground != null && tiles.TryGetValue(toTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return next(context).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemTransformCommand(toTile.Ground, toOpenTibiaId, 1) );
                } );
            }

            return next(context);
        }
    }
}