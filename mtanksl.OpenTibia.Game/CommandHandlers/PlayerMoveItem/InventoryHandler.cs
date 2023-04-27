using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Inventory toInventory)
            {
                Slot toSlot = (Slot)command.ToIndex;

                if (toSlot != Slot.Weapon && toSlot != Slot.Shield && toSlot != Slot.Extra)
                {
                    if (command.Item.Metadata.SlotType != toSlot)
                    {
                        Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotDressThisObjectThere) );

                        return Promise.Break;
                    }
                }
            }

            return next();
        }
    }
}