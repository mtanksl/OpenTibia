using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerAddItemUpdatePlayerCapacityHandler : CommandHandler<ContainerAddItemCommand>
    {
        public override Promise Handle(Func<Promise> next, ContainerAddItemCommand command)
        {
            if (command.Container.Root() is Inventory inventory)
            {
                return next().Then( () =>
                {
                    uint addWeight = command.Item.GetWeight();

                    return Context.AddCommand(new PlayerUpdateCapacityCommand(inventory.Player, (int)(inventory.Player.Capacity - addWeight) ) );
                } );
            }

            return next();
        }
    }
}