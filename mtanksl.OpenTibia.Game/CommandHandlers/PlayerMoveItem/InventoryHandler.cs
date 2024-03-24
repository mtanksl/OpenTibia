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

                SlotType slotType;

                if (command.Item.Metadata.SlotType == null)
                {
                    slotType = SlotType.Left | SlotType.Right | SlotType.Extra;
                }
                else
                {
                    slotType = command.Item.Metadata.SlotType.Value | SlotType.Left | SlotType.Right | SlotType.Extra;
                }

                bool proceed = false;

                switch (toSlot)
                {
                    case Slot.Head:

                        if (slotType.Is(SlotType.Head) )
                        {
                            proceed = true;
                        }

                        break;

                    case Slot.Amulet:

                        if (slotType.Is(SlotType.Amulet) )
                        {
                            proceed = true;
                        }

                        break;

                    case Slot.Container:

                        if (slotType.Is(SlotType.Container) )
                        {
                            proceed = true;
                        }

                        break;

                    case Slot.Armor:

                        if (slotType.Is(SlotType.Armor) )
                        {
                            proceed = true;
                        }

                        break;

                    case Slot.Right:

                        if (slotType.Is(SlotType.Right) )
                        {
                            Item left = toInventory.GetContent( (byte)Slot.Left) as Item;

                            if (slotType.Is(SlotType.TwoHand) )
                            {
                                if (left == null || left == command.Item)
                                {
                                    proceed = true;
                                }
                            }
                            else
                            {
                                if (left == null || left.Metadata.SlotType != SlotType.TwoHand )
                                {
                                    proceed = true;
                                }
                            }
                        }
                        
                        break;

                    case Slot.Left:

                        if (slotType.Is(SlotType.Left) )
                        {
                            Item right = toInventory.GetContent( (byte)Slot.Right) as Item;

                            if (slotType.Is(SlotType.TwoHand) )
                            {
                                if (right == null || right == command.Item)
                                {
                                    proceed = true;
                                }
                            }
                            else
                            {
                                if (right == null || right.Metadata.SlotType != SlotType.TwoHand )
                                {
                                    proceed = true;
                                }
                            }
                        }

                        break;

                    case Slot.Legs:

                        if (slotType.Is(SlotType.Legs) )
                        {
                            proceed = true;
                        }

                        break;

                    case Slot.Feet:

                        if (slotType.Is(SlotType.Feet) )
                        {
                            proceed = true;
                        }

                        break;

                    case Slot.Ring:

                        if (slotType.Is(SlotType.Ring) )
                        {
                            proceed = true;
                        }

                        break;

                    case Slot.Extra:

                        if (slotType.Is(SlotType.Extra) )
                        {
                            proceed = true;
                        }

                        break;
                }

                if (!proceed)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotDressThisObjectThere) );

                    return Promise.Break;
                }
            }

            return next();
        }
    }
}