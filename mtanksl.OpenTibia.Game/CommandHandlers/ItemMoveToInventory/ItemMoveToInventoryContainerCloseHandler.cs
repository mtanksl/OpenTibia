using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemMoveToInventoryContainerCloseHandler : CommandHandler<ItemMoveToTileCommand>
    {
        public override bool CanHandle(Context context, ItemMoveToTileCommand command)
        {
            return false;
        }

        public override void Handle(Context context, ItemMoveToTileCommand command)
        {
            
        }
    }
}