using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveCreatureWalkToSourceHandler : CommandHandler<PlayerMoveCreatureCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerMoveCreatureCommand command)
        {
            if ( !command.Player.Tile.Position.IsNextTo(command.ToTile.Position) )
            {
                IContainer beforeContainer = command.Creature.Parent;

                byte beforeIndex = beforeContainer.GetIndex(command.Creature);

                return context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, command.ToTile) ).Then(ctx =>
                {
                    return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

                } ).Then(ctx =>
                {
                    IContainer afterContainer = command.Creature.Parent;

                    byte afterIndex = afterContainer.GetIndex(command.Creature);

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