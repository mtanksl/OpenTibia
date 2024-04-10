using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithItemWalkToTargetHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (command.ToItem.Parent is Tile toTile && !command.Player.Tile.Position.IsNextTo(toTile.Position) )
            {
                if (command.Item.Parent is Tile || command.Item.Parent is Container container && !(container.Root() is Inventory) )
                {
                    return Context.AddCommand(new PlayerMoveItemCommand(command.Source, command.Player, command.Item, command.Player.Inventory, (byte)Slot.Extra, 1, false) ).Then( () =>
                    {
                        return Context.Server.GameObjectComponents.AddComponent(command.Player, new PlayerActionDelayBehaviour() ).Promise;

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, toTile) );

                    } ).Then( () =>
                    {
                        return Context.Server.GameObjectComponents.AddComponent(command.Player, new PlayerActionDelayBehaviour() ).Promise;

                    } ).Then( () =>
                    {
                        Item item = (Item)command.Player.Inventory.GetContent( (byte)Slot.Extra);

                        if (item == null || item.Metadata.OpenTibiaId != command.Item.Metadata.OpenTibiaId)
                        {
                            return Promise.Break;
                        }

                        return Context.AddCommand(new PlayerUseItemWithItemCommand(command.Source, command.Player, item, command.ToItem) );
                    } );
                }
                else
                {
                    IContainer beforeContainer = command.Item.Parent;

                    int beforeIndex = beforeContainer.GetIndex(command.Item);

                    return Context.AddCommand(new ParseWalkToUnknownPathCommand(command.Player, toTile) ).Then( () =>
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
            }

            return next();
        }
    }
}