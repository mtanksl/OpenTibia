using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfHolyWaterDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> lumpOfHolyWaterDoughs;
        private readonly HashSet<ushort> garlics;
        private readonly ushort lumpOfGarlicDough;

        public LumpOfHolyWaterDoughHandler()
        {
            lumpOfHolyWaterDoughs = Context.Server.Values.GetUInt16HashSet("values.items.lumpOfHolyWaterDoughs");
            garlics = Context.Server.Values.GetUInt16HashSet("values.items.garlics");
            lumpOfGarlicDough = Context.Server.Values.GetUInt16("values.items.lumpOfGarlicDough");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfHolyWaterDoughs.Contains(command.Item.Metadata.OpenTibiaId) && garlics.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new PlayerCreateItemCommand(command.Player, lumpOfGarlicDough, 1) );
                } );
            }

            return next();
        }
    }
}