using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BunchOfSugarCaneHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> bunchOfSugarCanes;
        private readonly Dictionary<ushort, ushort> distillingMachines;

        public BunchOfSugarCaneHandler()
        {
            bunchOfSugarCanes = Context.Server.Values.GetUInt16HashSet("values.items.bunchOfSugarCanes");
            distillingMachines = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.distillingMachines");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (bunchOfSugarCanes.Contains(command.Item.Metadata.OpenTibiaId) && distillingMachines.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                } );
            }

            return next();
        }
    }
}