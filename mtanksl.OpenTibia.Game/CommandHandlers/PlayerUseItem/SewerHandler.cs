using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SewerHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> sewers;

        public SewerHandler()
        {
            sewers = Context.Server.Values.GetUInt16HashSet("values.items.sewers");
        }

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