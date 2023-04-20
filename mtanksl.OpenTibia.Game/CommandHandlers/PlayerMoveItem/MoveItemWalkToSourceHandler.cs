using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveItemWalkToSourceHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                IContainer beforeContainer = command.Item.Parent;

                byte beforeIndex = beforeContainer.GetIndex(command.Item);

                byte beforeCount = command.Count;

                return Context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then( () =>
                {
                    return Context.Server.Components.AddComponent(command.Player, new PlayerActionDelayBehaviour() ).Promise;

                } ).Then( () =>
                {
                    IContainer afterContainer = command.Item.Parent;

                    if (beforeContainer != afterContainer)
                    {
                        return Promise.Break;
                    }

                    byte afterIndex = afterContainer.GetIndex(command.Item);

                    byte afterCount = command.Count;

                    if (beforeIndex != afterIndex || beforeCount != afterCount)
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