using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LadderHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> ladders;

        public LadderHandler()
        {
            ladders = Context.Server.Values.GetUInt16HashSet("values.items.ladders");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (ladders.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new CreatureMoveCommand(command.Player, Context.Server.Map.GetTile( ( (Tile)command.Item.Parent ).Position.Offset(0, 1, -1) ) ) ).Then( () =>
                {
                    return Context.AddCommand(new CreatureUpdateDirectionCommand(command.Player, Direction.South) );
                } );
            }

            return next();
        }
    }
}
