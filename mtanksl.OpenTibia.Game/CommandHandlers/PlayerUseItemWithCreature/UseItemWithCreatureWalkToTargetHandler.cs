using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithCreatureWalkToTargetHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if ( !command.Player.Tile.Position.IsNextTo(command.ToCreature.Tile.Position) )
            {
                if (command.Item.Parent is Tile || command.Item.Parent is Container container && container.Root() is Tile)
                {
                    return context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, command.Player.Inventory, (byte)Slot.Extra, 1) ).Then(ctx =>
                    {
                        return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

                    } ).Then(ctx =>
                    {
                        return ctx.AddCommand(new WalkToUnknownPathCommand(command.Player, command.ToCreature.Tile) );

                    } ).Then(ctx =>
                    {
                        return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

                    } ).Then(ctx =>
                    {
                        IContainer afterContainer = command.Item.Parent;

                        byte afterIndex = afterContainer.GetIndex(command.Item);

                        if (afterContainer is Inventory && afterIndex == (byte)Slot.Extra)
                        {
                            return next(ctx);
                        }

                        return Promise.FromResult(ctx);
                    } );
                }
                else
                {
                    IContainer beforeContainer = command.Item.Parent;

                    byte beforeIndex = beforeContainer.GetIndex(command.Item);

                    return context.AddCommand(new WalkToUnknownPathCommand(command.Player, command.ToCreature.Tile) ).Then(ctx =>
                    {
                        return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

                    } ).Then(ctx =>
                    {
                        IContainer afterContainer = command.Item.Parent;

                        byte afterIndex = afterContainer.GetIndex(command.Item);

                        if (beforeContainer == afterContainer && beforeIndex == afterIndex)
                        {
                            return next(ctx);
                        }

                        return Promise.FromResult(ctx);
                    } );
                }
            }

            return next(context);
        }
    }
}