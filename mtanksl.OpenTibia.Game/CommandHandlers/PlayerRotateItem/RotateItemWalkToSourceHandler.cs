﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemWalkToSourceHandler : CommandHandler<PlayerRotateItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerRotateItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                IContainer beforeContainer = command.Item.Parent;

                int beforeIndex = beforeContainer.GetIndex(command.Item);

                return Context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then( () =>
                {
                    return Context.Server.GameObjectComponents.AddComponent(command.Player, new PlayerActionDelayBehaviour() ).Promise;

                } ).Then( () =>
                {
                    IContainer afterContainer = command.Item.Parent;

                    if (beforeContainer != afterContainer)
                    {
                        return Promise.Break;
                    }

                    int afterIndex = afterContainer.GetIndex(command.Item);

                    if (beforeIndex != afterIndex)
                    {
                        return Promise.Break;
                    }

                    if (command.Source == null)
                    {
                        return Promise.Break;
                    }

                    return Context.AddCommand(command.Source);
                } );
            }

            return next();
        }
    }
}