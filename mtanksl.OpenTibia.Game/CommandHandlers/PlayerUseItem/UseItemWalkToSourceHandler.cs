using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWalkToSourceHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                IContainer beforeContainer = command.Item.Parent;

                byte beforeIndex = beforeContainer.GetIndex(command.Item);

                return Context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then( () =>
                {
                    return Context.Server.Components.AddComponent(command.Player, new PlayerActionBehaviour() ).Promise;

                } ).Then( () =>
                {
                    IContainer afterContainer = command.Item.Parent;

                    byte afterIndex = afterContainer.GetIndex(command.Item);

                    if (beforeContainer != afterContainer || beforeIndex != afterIndex)
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