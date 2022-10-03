using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemMoveToInventoryContainerCloseHandler : CommandHandler<ItemUpdateParentToInventoryCommand>
    {
        public override bool CanHandle(Context context, ItemUpdateParentToInventoryCommand command)
        {
            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToInventoryCommand command)
        {
            
        }
    }
}