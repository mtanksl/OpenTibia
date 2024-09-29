using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> lumpOfDoughs;
        private readonly HashSet<ushort> ovens;
        private readonly ushort bread;

        public LumpOfDoughHandler()
        {
            lumpOfDoughs = Context.Server.Values.GetUInt16HashSet("values.items.lumpOfDoughs");
            ovens = Context.Server.Values.GetUInt16HashSet("values.items.ovens");
            bread = Context.Server.Values.GetUInt16("values.items.bread");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfDoughs.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, bread, 1) );
                } );
            }

            return next();
        }
    }
}