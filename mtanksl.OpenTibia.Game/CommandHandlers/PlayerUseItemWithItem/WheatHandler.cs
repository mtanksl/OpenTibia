using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WheatHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> wheats;
        private readonly HashSet<ushort> millstones;
        private readonly ushort flour;

        public WheatHandler()
        {
            wheats = Context.Server.Values.GetUInt16HashSet("values.items.wheats");
            millstones = Context.Server.Values.GetUInt16HashSet("values.items.millstones");
            flour = Context.Server.Values.GetUInt16("values.items.flour");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (wheats.Contains(command.Item.Metadata.OpenTibiaId) && millstones.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                {
                    return Context.AddCommand(new PlayerCreateItemCommand(command.Player, flour, 1) );
                } );
            }

            return next();
        }
    }
}