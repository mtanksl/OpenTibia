using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SewerHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> sewers = new HashSet<ushort>() { 430 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (sewers.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new CreatureMoveCommand(command.Player, Context.Server.Map.GetTile( ( (Tile)command.Item.Parent ).Position.Offset(0, 0, 1) ) ) ).Then( () =>
                {
                    return Context.AddCommand(new CreatureUpdateDirectionCommand(command.Player, Direction.South) );
                } );
            }

            return next();
        }
    }
}