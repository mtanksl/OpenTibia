using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemTransformUpdatePlayerCapacityHandler : CommandResultHandler<Item, ItemTransformCommand>
    {
        public override PromiseResult<Item> Handle(Func<PromiseResult<Item>> next, ItemTransformCommand command)
        {
            if (command.Item.Root() is Inventory inventory)
            {
                uint removeWeight = command.Item.GetWeight();

                return next().Then( (toItem) =>
                {
                    uint addWeight = toItem.GetWeight();

                    return Context.AddCommand(new PlayerUpdateCapacityCommand(inventory.Player, (int)(inventory.Player.Capacity + removeWeight - addWeight) ) ).Then( () =>
                    {
                        return Promise.FromResult(toItem);
                    } );
                } );
            }

            return next();
        }
    }
}