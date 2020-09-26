using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SewerHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> sewers = new HashSet<ushort>() { 430 };

        public override bool CanHandle(PlayerUseItemCommand command, Context context)
        {
            if (sewers.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Context context)
        {
            return new CreatureMoveCommand(command.Player, context.Server.Map.GetTile( ( (Tile)command.Item.Container ).Position.Offset(0, 0, 1) ) );
        }
    }
}