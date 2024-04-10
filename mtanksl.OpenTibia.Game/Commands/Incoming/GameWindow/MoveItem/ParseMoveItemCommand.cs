using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

        protected bool IsPossible(Item fromItem, Container toContainer)
        {
            if (fromItem is Container fromContainer)
            {
                if (toContainer.IsContentOf(fromContainer) )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );

                    return false;
                }
            }

            return true;
        }

        protected bool IsPickupable(Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                return false;
            }

            return true;
        }

        protected bool IsMoveable(Item fromItem, byte count)
        {
            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

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
            if (fromCreature is Npc || fromCreature is Monster)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

                return false;
            }

            return true;
        }        
    }
}