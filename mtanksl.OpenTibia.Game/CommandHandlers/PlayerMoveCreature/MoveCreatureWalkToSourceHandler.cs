using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveCreatureWalkToSourceHandler : CommandHandler<PlayerMoveCreatureCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveCreatureCommand command)
        {
            if ( !command.Player.Tile.Position.IsNextTo(command.Creature.Tile.Position) )
            {
                IContainer beforeContainer = command.Creature.Parent;

                byte beforeIndex = beforeContainer.GetIndex(command.Creature);

                return Context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, command.Creature.Tile) ).Then( () =>
                {
                    return Promise.Delay(Context.Server, Constants.PlayerAutomationSchedulerEvent(command.Player), Constants.PlayerAutomationSchedulerEventInterval);

                } ).Then( () =>
                {
                    IContainer afterContainer = command.Creature.Parent;

                    byte afterIndex = afterContainer.GetIndex(command.Creature);

                    if (beforeContainer == afterContainer && beforeIndex == afterIndex)
                    {
                        return next();
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}