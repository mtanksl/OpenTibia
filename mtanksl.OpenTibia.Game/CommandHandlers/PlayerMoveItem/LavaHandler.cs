using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LavaHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> lavas = new HashSet<ushort>() { 598, 599, 600, 601 };

        public override Promise Handle(ContextPromiseDelegate next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && lavas.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.FirePlume) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemDecrementCommand(command.Item, command.Count) );
                } );
            }

            return next(context);
        }
    }
}