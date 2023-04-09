using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveItemWalkToSourceHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(ContextPromiseDelegate next, PlayerMoveItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                IContainer beforeContainer = command.Item.Parent;

                byte beforeIndex = beforeContainer.GetIndex(command.Item);

                byte beforeCount = command.Count;

                return context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then(ctx =>
                {
                    return Promise.Delay(ctx.Server, Constants.PlayerAutomationSchedulerEvent(command.Player), Constants.PlayerAutomationSchedulerEventInterval);

                } ).Then(ctx =>
                {
                    IContainer afterContainer = command.Item.Parent;

                    byte afterIndex = afterContainer.GetIndex(command.Item);

                    byte afterCount = command.Count;

                    if (beforeContainer == afterContainer && beforeIndex == afterIndex && beforeCount == afterCount)
                    {
                        return next(ctx);
                    }

                    return Promise.Completed(ctx);
                } );
            }

            return next(context);
        }
    }
}