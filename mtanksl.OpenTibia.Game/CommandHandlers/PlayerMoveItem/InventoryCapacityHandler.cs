using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryCapacityHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if ( (command.ToContainer is Inventory || (command.ToContainer is Container toContainer && toContainer.Root() is Inventory) ) && !(command.Item.Root() is Inventory) )
            {
                uint weight;

                if (command.Item is StackableItem stackableItem)
                {
                    weight = command.Count * (stackableItem.Metadata.Weight ?? 0);
                }
                else
                {
                    weight = command.Item.GetWeight();
                }

                uint capacity = command.Player.Capacity;

                if (weight > capacity)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisObjectIsTooHeavy) );

                    return Promise.Break;
                }
            }

            return next();
        }
    }
}