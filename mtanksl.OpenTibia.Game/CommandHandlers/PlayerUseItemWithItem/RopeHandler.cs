using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RopeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> ropes = new HashSet<ushort>() { 2120 };

        private HashSet<ushort> ropeSpots = new HashSet<ushort> { 384 };

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (ropes.Contains(command.Item.Metadata.OpenTibiaId) && ropeSpots.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            context.AddCommand(new CreatureMoveCommand(command.Player, command.Player.Tile, context.Server.Map.GetTile( ( (Tile)command.ToItem.Parent ).Position.Offset(0, 1, -1) ) ) );

            base.Handle(context, command);
        }
    }
}