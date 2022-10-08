using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileDepressHandler : CommandHandler<CreatureUpdateParentCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 417, 416 },
            { 425, 426 },
            { 447, 446 },
            { 3217, 3216 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, CreatureUpdateParentCommand command)
        {
            ushort toOpenTibiaId;
            
            Tile fromTile = command.Creature.Tile;

            if (fromTile.Ground != null && tiles.TryGetValue(fromTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return next(context).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemTransformCommand(fromTile.Ground, toOpenTibiaId, 1) );
                } );
            }

            return next(context);
        }
    }
}