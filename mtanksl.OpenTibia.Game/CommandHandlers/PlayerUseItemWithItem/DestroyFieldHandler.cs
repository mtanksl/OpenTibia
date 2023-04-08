using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DestroyFieldHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> destroyFields = new HashSet<ushort>() { 2261 };

        private HashSet<ushort> fields = new HashSet<ushort>() { 1492, 1493, 1494, 1495, 1496 };

        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerUseItemWithItemCommand command)
        {
            if (destroyFields.Contains(command.Item.Metadata.OpenTibiaId) && fields.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new  ShowMagicEffectCommand( ( (Tile)command.ToItem.Parent).Position, MagicEffectType.Puff) ).Then(ctx =>
                {
                    return context.AddCommand(new ItemDestroyCommand(command.ToItem) );

                } ).Then(ctx =>
                {
                    return context.AddCommand(new ItemDecrementCommand(command.Item, 1) );
                } );
            }

            return next(context);
        }
    }
}