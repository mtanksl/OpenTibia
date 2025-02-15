using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class StackableItemUpdateCountUpdatePlayerCapacitytHandler : CommandHandler<StackableItemUpdateCountCommand>
    {
        public override Promise Handle(Func<Promise> next, StackableItemUpdateCountCommand command)
        {
            if (command.StackableItem.Root() is Inventory inventory)
            {
                uint removeWeight = command.StackableItem.GetWeight();

                return next().Then( () =>
                {
                    uint addWeight = command.StackableItem.GetWeight();

                    return Context.AddCommand(new PlayerUpdateCapacityCommand(inventory.Player, (int)(inventory.Player.Capacity - addWeight + removeWeight) ) );
                } );
            }

            return next();
        }
    }
}