using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
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
                    return Context.Server.Components.AddComponent(command.Player, new PlayerActionDelayBehaviour() ).Promise;

                } ).Then( () =>
                {
                    IContainer afterContainer = command.Creature.Parent;

                    if (beforeContainer != afterContainer)
                    {
                        return Promise.Break;
                    }

                    byte afterIndex = afterContainer.GetIndex(command.Creature);

                    if (beforeIndex != afterIndex)
                    {
                        return Promise.Break;
                    }

                    return next();
                } );
            }

            return next();
        }
    }
}