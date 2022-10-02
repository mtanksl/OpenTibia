using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveItemHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            return true;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            switch (command.ToContainer)
            {
                case Tile toTile:

                    context.AddCommand(new ItemMoveToTileCommand(command.Item, toTile) ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Inventory toInventory:

                    context.AddCommand(new ItemMoveToInventoryCommand(command.Item, toInventory, command.ToIndex) ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Container toContainer:

                    context.AddCommand(new ItemMoveToContainerCommand(command.Item, toContainer) ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;
            }
        }
    }
}