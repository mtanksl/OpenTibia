using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PickHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> picks = new HashSet<ushort>() { 2553 };

        private Dictionary<ushort, ushort> fragileIces = new Dictionary<ushort, ushort>()
        {
            { 7200, 7236 },
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 7236, 7200 }
        };

        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (picks.Contains(command.Item.Metadata.OpenTibiaId) && fragileIces.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new ShowMagicEffectCommand( ( (Tile)command.ToItem.Parent).Position, MagicEffectType.Puff) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                } ).Then( (ctx, item) =>
                {
                    return ctx.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[item.Metadata.OpenTibiaId], 1) );
                } );
            }

            return next(context);
        }
    }
}