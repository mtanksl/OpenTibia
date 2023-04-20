using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BunchOfSugarCaneHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> bunchOfSugarCanes = new HashSet<ushort>() { 5467 };

        private Dictionary<ushort, ushort> distillingMachines = new Dictionary<ushort, ushort>()
        {
            { 5469, 5513 },
            { 5470, 5514 },
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 5513, 5469 },
            { 5514, 5470 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (bunchOfSugarCanes.Contains(command.Item.Metadata.OpenTibiaId) && distillingMachines.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}