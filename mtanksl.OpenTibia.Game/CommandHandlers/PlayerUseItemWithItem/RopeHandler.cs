using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RopeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> ropes = new HashSet<ushort>() { 2120 };

        private HashSet<ushort> ropeSpots = new HashSet<ushort> { 384 };

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (ropes.Contains(command.Item.Metadata.OpenTibiaId) && ropeSpots.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            return new CreatureMoveCommand(command.Player, server.Map.GetTile( ( (Tile)command.ToItem.Container ).Position.Offset(0, 1, -1) ) );
        }
    }
}