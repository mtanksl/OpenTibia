using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MacheteHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> machetes = new HashSet<ushort>() { 2420 };

        private Dictionary<ushort, ushort> jungleGrass = new Dictionary<ushort, ushort>()
        {
            { 2782, 2781 },
            { 3985, 3984 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 2781, 2782 },
            { 3984, 3985 }
        };

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (machetes.Contains(command.Item.Metadata.OpenTibiaId) && jungleGrass.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                {
                    return ctx.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[item.Metadata.OpenTibiaId], 1) );
                } );
            }

            return next(context);
        }
    }
}