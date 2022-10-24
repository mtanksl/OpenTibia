using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class SplitStackableItemHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile)
            {
                if (command.Item is StackableItem fromStackableItem)
                {
                    if (toTile.TopItem is StackableItem toStackableItem && toStackableItem.Metadata.OpenTibiaId == fromStackableItem.Metadata.OpenTibiaId)
                    {
                        if (toStackableItem.Count + command.Count > 100)
                        {
                            context.AddCommand(new TileCreateItemCommand(toTile, fromStackableItem.Metadata.OpenTibiaId, (byte)(toStackableItem.Count + command.Count - 100) ) );

                            context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );

                            context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                        }
                        else
                        {
                            context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + command.Count) ) );

                            context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                        }
                    }
                    else
                    {
                        context.AddCommand(new TileCreateItemCommand(toTile, fromStackableItem.Metadata.OpenTibiaId, (byte)(command.Count) ) );

                        context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                    }

                    return Promise.FromResult(context);
                }
            }

            return next(context);
        }
    }
}