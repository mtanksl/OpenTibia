using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseMoveItemCommand : Command
    {
        public ParseMoveItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsMoveable(Context context, Item fromItem, byte count)
        {
            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

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

        protected bool IsMoveable(Context context, Creature fromCreature)
        {
            return true;
        }

        protected bool IsPickupable(Context context, Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                return false;
            }

            return true;
        }

        protected bool IsPossible(Context context, Item fromItem, Container toContainer)
        {
            if ( toContainer.IsContentOf(fromItem) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );

                return false;
            }

            return true;
        }
    }
}