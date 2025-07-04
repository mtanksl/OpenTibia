﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseMoveItemCommand : IncomingCommand
    {
        public ParseMoveItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsPossible(Item fromItem, Container toContainer, byte toIndex)
        {
            if (fromItem is Container container)
            {
                if (toContainer.GetContent(toIndex) == container || toContainer.IsContentOf(container) )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.ThisIsImpossible) );

                    return false;
                }
            }

            return true;
        }

        protected bool IsPossible(Item fromItem, Inventory toInventory, byte toSlot)
        {
            if (fromItem is Container container)
            {
                if (toInventory.GetContent(toSlot) == container)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.ThisIsImpossible) );

                    return false;
                }
            }

            return true;
        }

        protected bool IsPickupable(Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouCanNotTakeThisObject) );

                return false;
            }

            return true;
        }

        protected bool IsMoveable(Item fromItem, byte count)
        {
            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouCanNotMoveThisObject) );

                return false;
            }

            if (fromItem is StackableItem stackableItem)
            {
                if (count < 1 || count > stackableItem.Count)
                {
                    return false;
                }
            }
            else
            {
                if (count != 1)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool IsMoveable(Creature fromCreature)
        {
            if (fromCreature is Player || fromCreature is Npc || fromCreature is Monster)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouCanNotMoveThisObject) );

                return false;
            }

            return true;
        }
    }
}