using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WheatHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> wheats = new HashSet<ushort>() { 2694 };

        private HashSet<ushort> millstones = new HashSet<ushort>() { 1381, 1382, 1383, 1384 };

        private ushort flour = 2692;

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