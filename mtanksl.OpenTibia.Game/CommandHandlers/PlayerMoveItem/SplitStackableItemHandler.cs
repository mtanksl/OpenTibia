using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class SplitStackableItemHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerMoveItemCommand command)
        {
            if (command.Item is StackableItem stackableItem)
            {
                if (command.Count < stackableItem.Count)
                {
                    switch (command.ToContainer)
                    {
                        case Container container:

                            return context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count - command.Count) ) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new ContainerCreateItemCommand(container, command.Item.Metadata.OpenTibiaId, command.Count) );
                            } );

                        case Inventory inventory:

                            return context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count - command.Count) ) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new InventoryCreateItemCommand(inventory, command.ToIndex, command.Item.Metadata.OpenTibiaId, command.Count) );
                            } );

                        case Tile tile:

                            return context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count - command.Count) ) ).Then(ctx =>
                            {
                                return context.AddCommand(new TileCreateItemCommand(tile, command.Item.Metadata.OpenTibiaId, command.Count));
                            } );
                    }
                }
            }

            return next(context);
        }
    }
}