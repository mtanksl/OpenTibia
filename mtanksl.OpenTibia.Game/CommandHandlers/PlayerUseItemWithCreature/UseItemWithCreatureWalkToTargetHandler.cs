using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithCreatureWalkToTargetHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerUseItemWithCreatureCommand command)
        {
            if ( !command.Player.Tile.Position.IsNextTo(command.ToCreature.Tile.Position) )
            {
                if (command.Item.Parent is Tile || command.Item.Parent is Container container && container.Root() is Tile)
                {
                    return context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, command.Player.Inventory, (byte)Slot.Extra, 1, false) ).Then(ctx =>
                    {
                        return Promise.Delay(ctx.Server, Constants.PlayerAutomationSchedulerEvent(command.Player), Constants.PlayerAutomationSchedulerEventInterval);

                    } ).Then(ctx =>
                    {
                        return ctx.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, command.ToCreature.Tile) );

                    } ).Then(ctx =>
                    {
                        return Promise.Delay(ctx.Server, Constants.PlayerAutomationSchedulerEvent(command.Player), Constants.PlayerAutomationSchedulerEventInterval);

                    } ).Then(ctx =>
                    {
                        Item item = command.Player.Inventory.GetContent( (byte)Slot.Extra) as Item;

                        if (item != null && item.Metadata.OpenTibiaId == command.Item.Metadata.OpenTibiaId)
                        {
                            return ctx.AddCommand(new PlayerUseItemWithCreatureCommand(command.Player, item, command.ToCreature) );
                        }

                        return Promise.FromResult(ctx);
                    } );
                }
                else
                {
                    IContainer beforeContainer = command.Item.Parent;

                    byte beforeIndex = beforeContainer.GetIndex(command.Item);

                    return context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, command.ToCreature.Tile) ).Then(ctx =>
                    {
                        return Promise.Delay(ctx.Server, Constants.PlayerAutomationSchedulerEvent(command.Player), Constants.PlayerAutomationSchedulerEventInterval);

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