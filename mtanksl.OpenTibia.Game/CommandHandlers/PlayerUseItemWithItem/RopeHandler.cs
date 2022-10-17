using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RopeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> ropes = new HashSet<ushort>() { 2120 };

        private HashSet<ushort> ropeSpots = new HashSet<ushort> { 384, 418 };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (ropes.Contains(command.Item.Metadata.OpenTibiaId) && ropeSpots.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new CreatureUpdateParentCommand(command.Player, context.Server.Map.GetTile( ( (Tile)command.ToItem.Parent ).Position.Offset(0, 1, -1) ) ) );
            }

            return next(context);
        }
    }
}