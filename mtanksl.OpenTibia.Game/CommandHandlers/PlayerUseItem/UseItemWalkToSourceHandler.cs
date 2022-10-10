using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWalkToSourceHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                IContainer beforeContainer = command.Item.Parent;

                byte beforeIndex = beforeContainer.GetIndex(command.Item);

                return context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then(ctx =>
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

            return next(context);
        }
    }
}