using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithItemWalkToTargetHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (command.ToItem.Parent is Tile toTile && !command.Player.Tile.Position.IsNextTo(toTile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (command.Item.Parent is Tile || command.Item.Parent is Container container && container.Root() is Tile)
            {
                context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, command.Player.Inventory, (byte)Slot.Extra, 1) ).Then(ctx =>
                {
                    return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.ToItem.Parent) );

                } ).Then(ctx =>
                {
                    return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

                } ).Then(ctx =>
                {
                    IContainer afterContainer = command.Item.Parent;

                    byte afterIndex = afterContainer.GetIndex(command.Item);

                    if (afterContainer is Inventory && afterIndex == (byte)Slot.Extra)
                    {
                        return ctx.AddCommand(command);
                    }

                    return null;

                } ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
            else
            {
                IContainer beforeContainer = command.Item.Parent;

                byte beforeIndex = beforeContainer.GetIndex(command.Item);

                context.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.ToItem.Parent) ).Then(ctx =>
                {
                    return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

                } ).Then(ctx =>
                {
                    IContainer afterContainer = command.Item.Parent;

                    byte afterIndex = afterContainer.GetIndex(command.Item);

                    if (beforeContainer == afterContainer && beforeIndex == afterIndex)
                    {
                        return ctx.AddCommand(command);
                    }

                    return null;

                } ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
        }
    }
}