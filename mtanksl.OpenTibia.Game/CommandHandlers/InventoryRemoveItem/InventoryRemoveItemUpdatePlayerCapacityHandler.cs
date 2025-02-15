using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryRemoveItemUpdatePlayerCapacityHandler : CommandHandler<InventoryRemoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, InventoryRemoveItemCommand command)
        {
            uint removeWeight = command.Item.GetWeight();

            return next().Then( () =>
            {
                return Context.AddCommand(new PlayerUpdateCapacityCommand(command.Inventory.Player, (int)(command.Inventory.Player.Capacity + removeWeight) ) );
            } );
        }
    }
}