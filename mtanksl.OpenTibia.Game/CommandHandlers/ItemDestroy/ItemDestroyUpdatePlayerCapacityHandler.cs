using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemDestroyUpdatePlayerCapacityHandler : CommandHandler<ItemDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, ItemDestroyCommand command)
        {
            if (command.Item.Root() is Inventory inventory)
            {
                uint removeWeight = command.Item.GetWeight();

                return next().Then( () =>
                {
                    return Context.AddCommand(new PlayerUpdateCapacityCommand(inventory.Player, (int)(inventory.Player.Capacity + removeWeight) ) );
                } );
            }

            return next();
        }
    }
}