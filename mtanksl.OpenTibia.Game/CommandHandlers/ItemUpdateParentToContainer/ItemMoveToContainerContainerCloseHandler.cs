using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemMoveToContainerContainerCloseHandler : CommandHandler<ItemUpdateParentToContainerCommand>
    {
        public override bool CanHandle(Context context, ItemUpdateParentToContainerCommand command)
        {
            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToContainerCommand command)
        {
            
        }
    }
}