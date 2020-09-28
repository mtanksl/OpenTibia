using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WheatHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            return null;
        }
    }
}