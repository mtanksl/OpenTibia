using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LadderHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> ladders = new HashSet<ushort>() { 1386, 3678, 5543 };

        public override bool CanHandle(PlayerUseItemCommand command, Context context)
        {
            if (ladders.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Context context)
        {
            return new CreatureMoveCommand(command.Player, context.Server.Map.GetTile( ( (Tile)command.Item.Container ).Position.Offset(0, 1, -1) ) );
        }
    }
}