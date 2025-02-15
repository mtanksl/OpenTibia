using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryAddItemUpdatePlayerCapacityHandler : CommandHandler<InventoryAddItemCommand>
    {
        public override Promise Handle(Func<Promise> next, InventoryAddItemCommand command)
        {
            return next().Then( () =>
            {
                uint addWeight = command.Item.GetWeight();

                return Context.AddCommand(new PlayerUpdateCapacityCommand(command.Inventory.Player, (int)(command.Inventory.Player.Capacity - addWeight) ) );
            } );
        }
    }
}