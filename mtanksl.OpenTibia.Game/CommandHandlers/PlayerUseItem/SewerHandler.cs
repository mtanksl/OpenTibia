using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SewerHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> sewers = new HashSet<ushort>() { 430 };

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            if (sewers.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new CreatureUpdateParentCommand(command.Player, context.Server.Map.GetTile( ( (Tile)command.Item.Parent ).Position.Offset(0, 0, 1) ), Direction.South) );
            }

            return next(context);
        }
    }
}