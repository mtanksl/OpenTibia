using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryDressHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Inventory toInventory)
            {
                Slot toSlot = (Slot)command.ToIndex;

                if (toInventory.GetContent( (int)toSlot) == null)
                {
                    SlotType slotType;

                    if (command.Item.Metadata.SlotType == null)
                    {
                        slotType = SlotType.Left | SlotType.Right | SlotType.Ammo;
                    }
                    else
                    {
                        slotType = command.Item.Metadata.SlotType.Value | SlotType.Left | SlotType.Right | SlotType.Ammo;
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

                        case Slot.Necklace:

                            if (slotType.Is(SlotType.Necklace) )
                            {
                                proceed = true;
                            }

                            break;

                        case Slot.Backpack:

                            if (slotType.Is(SlotType.Backpack) )
                            {
                                proceed = true;
                            }

                            break;

                        case Slot.Body:

                            if (slotType.Is(SlotType.Body) )
                            {
                                proceed = true;
                            }

                            break;

                        case Slot.Right:

                            if (slotType.Is(SlotType.Right) )
                            {
                                Item left = (Item)toInventory.GetContent( (byte)Slot.Left);

                                if (left == null)
                                {
                                    proceed = true;
                                }
                                else if (left == command.Item)
                                {
                                    if (left is StackableItem stackableItem)
                                    {
                                        if (stackableItem.Count == command.Count || left.Metadata.WeaponType != WeaponType.Distance)
                                        {
                                            proceed = true;
                                        }
                                    }
                                    else
                                    {
                                        proceed = true;
                                    }
                                }
                                else if ( !slotType.Is(SlotType.TwoHanded) && 

                                          left.Metadata.SlotType != SlotType.TwoHanded && 

                                          !(command.Item.Metadata.WeaponType == WeaponType.Shield && left.Metadata.WeaponType == WeaponType.Shield) &&

                                          !( (command.Item.Metadata.WeaponType == WeaponType.Sword || command.Item.Metadata.WeaponType == WeaponType.Club || command.Item.Metadata.WeaponType == WeaponType.Axe || command.Item.Metadata.WeaponType == WeaponType.Distance || command.Item.Metadata.WeaponType == WeaponType.Wand) && (left.Metadata.WeaponType == WeaponType.Sword || left.Metadata.WeaponType == WeaponType.Club || left.Metadata.WeaponType == WeaponType.Axe || left.Metadata.WeaponType == WeaponType.Distance || left.Metadata.WeaponType == WeaponType.Wand) ) )
                                {
                                    proceed = true;
                                }
                            }

                            break;

                        case Slot.Left:

                            if (slotType.Is(SlotType.Left) )
                            {
                                Item right = (Item)toInventory.GetContent( (byte)Slot.Right);

                                if (right == null)
                                {
                                    proceed = true;
                                }
                                else if (right == command.Item)
                                {
                                    if (right is StackableItem stackableItem)
                                    {
                                        if (stackableItem.Count == command.Count || right.Metadata.WeaponType != WeaponType.Distance)
                                        {
                                            proceed = true;
                                        }
                                    }
                                    else
                                    {
                                        proceed = true;
                                    }
                                }
                                else if ( !slotType.Is(SlotType.TwoHanded) &&

                                          right.Metadata.SlotType != SlotType.TwoHanded && 

                                          !(command.Item.Metadata.WeaponType == WeaponType.Shield && right.Metadata.WeaponType == WeaponType.Shield) &&

                                          !( (command.Item.Metadata.WeaponType == WeaponType.Sword || command.Item.Metadata.WeaponType == WeaponType.Club || command.Item.Metadata.WeaponType == WeaponType.Axe || command.Item.Metadata.WeaponType == WeaponType.Distance || command.Item.Metadata.WeaponType == WeaponType.Wand) && (right.Metadata.WeaponType == WeaponType.Sword || right.Metadata.WeaponType == WeaponType.Club || right.Metadata.WeaponType == WeaponType.Axe || right.Metadata.WeaponType == WeaponType.Distance || right.Metadata.WeaponType == WeaponType.Wand) ) )
                                {
                                    proceed = true;
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

                        case Slot.Ammo:

                            if (slotType.Is(SlotType.Ammo) )
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
            }

            return next();
        }
    }
}